using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Demo.Infrastructure.Tools;

/// <summary>
/// 库存预警消息（由 Inventory_Service 发布，StockAlert_HostedService 消费）。
/// MessageId 用于消费端 Redis 幂等去重。
/// </summary>
public class Stock_Alert_Message
{
    public string? MessageId { get; set; }
    public long MaterialId { get; set; }
    public string? MaterialCode { get; set; }
    public string? MaterialName { get; set; }
    public long WarehouseId { get; set; }
    public decimal CurrentQty { get; set; }
    public decimal SafetyStock { get; set; }
}

/// <summary>
/// RabbitMQ 消息总线。
/// 连接与通道按需创建，Host 未配置或连接失败时 Enabled=false，不影响服务启动。
/// 主队列 erp.stock.alert 绑定死信交换器，消费失败的消息 Nack(requeue:false) 进入 erp.stock.alert.dlq。
/// 消息持久化，消费端支持 CancellationToken 取消。
/// </summary>
public sealed class RabbitMq_Bus : IDisposable
{
    public const string StockAlertQueue = "erp.stock.alert";
    public const string StockAlertDlq = "erp.stock.alert.dlq";
    private const string DeadLetterExchange = "erp.dlx";

    private readonly IConnection? _conn;
    private readonly IModel? _ch;
    private readonly bool _enabled;
    private readonly ILogger<RabbitMq_Bus> _log;
    private bool _disposed;

    public RabbitMq_Bus(IConfiguration config, ILogger<RabbitMq_Bus> log)
    {
        _log = log;
        var host = config["RabbitMQ:Host"];
        _enabled = !string.IsNullOrWhiteSpace(host);
        if (!_enabled)
        {
            _log.LogWarning("RabbitMQ:Host 未配置，消息总线已禁用（库存预警将仅记录日志，不异步投递）");
            return;
        }

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = host,
                Port = int.TryParse(config["RabbitMQ:Port"], out var p) ? p : 5672,
                UserName = config["RabbitMQ:User"] ?? "guest",
                Password = config["RabbitMQ:Password"] ?? "guest",
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };
            _conn = factory.CreateConnection();
            _ch = _conn.CreateModel();
            DeclareTopology();
            _log.LogInformation("RabbitMQ 连接成功，交换机/队列已就绪");
        }
        catch (Exception ex)
        {
            // 降级：连接失败不阻断 Web 启动，仅记录并禁用总线
            _enabled = false;
            _log.LogError(ex, "RabbitMQ 连接失败，消息总线已降级为禁用状态");
            _ch?.Dispose();
            _conn?.Dispose();
        }
    }

    public bool Enabled => _enabled && _ch != null;

    private void DeclareTopology()
    {
        if (_ch is null) return;
        // 死信交换器 + 死信队列
        _ch.ExchangeDeclare(DeadLetterExchange, ExchangeType.Fanout, durable: true);
        _ch.QueueDeclare(StockAlertDlq, durable: true, exclusive: false, autoDelete: false);
        _ch.QueueBind(StockAlertDlq, DeadLetterExchange, routingKey: string.Empty);

        // 主队列：消息失败后（requeue:false）经死信交换器进入 DLQ
        var args = new Dictionary<string, object?> { ["x-dead-letter-exchange"] = DeadLetterExchange };
        _ch.QueueDeclare(StockAlertQueue, durable: true, exclusive: false, autoDelete: false, arguments: args);
    }

    /// <summary>发布消息（持久化）。总线禁用时静默丢弃，由调用方降级处理。</summary>
    public void Publish<T>(string queue, T message)
    {
        if (_ch is null) return;
        try
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = _ch.CreateBasicProperties();
            props.Persistent = true;
            props.ContentType = "application/json";
            if (message is Stock_Alert_Message m && !string.IsNullOrWhiteSpace(m.MessageId))
                props.MessageId = m.MessageId;
            _ch.BasicPublish(exchange: string.Empty, routingKey: queue, basicProperties: props, body: body);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "RabbitMQ 发布消息失败 queue={Queue}", queue);
            throw;
        }
    }

    /// <summary>
    /// 启动异步消费者。返回 IDisposable 句柄，释放（或停机）时注销消费者，实现优雅停机。
    /// handler 处理成功返回即 Ack；抛异常则 Nack(requeue:false) 进入死信队列。
    /// </summary>
    public IDisposable Consume(string queue, Func<string, CancellationToken, Task> handler, CancellationToken stoppingToken)
    {
        if (_ch is null)
            return NullDisposable.Instance;

        var consumer = new AsyncEventingBasicConsumer(_ch);
        consumer.Received += async (_, ea) =>
        {
            if (stoppingToken.IsCancellationRequested) return;
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                await handler(json, stoppingToken);
                _ch.BasicAck(ea.DeliveryTag, false);
            }
            catch (OperationCanceledException)
            {
                _ch.BasicNack(ea.DeliveryTag, false, false);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "库存预警消息处理失败，转入死信队列 deliveryTag={Tag}", ea.DeliveryTag);
                _ch.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _ch.BasicQos(0, prefetchCount: 16, false);
        var consumerTag = _ch.BasicConsume(queue, autoAck: false, consumer: consumer);
        _log.LogInformation("RabbitMQ 消费者已启动 queue={Queue} tag={Tag}", queue, consumerTag);

        return new ConsumerHandle(_ch, consumerTag, stoppingToken);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        try { _ch?.Dispose(); } catch { /* ignore */ }
        try { _conn?.Dispose(); } catch { /* ignore */ }
    }

    private sealed class ConsumerHandle : IDisposable
    {
        private readonly IModel _ch;
        private readonly string _tag;
        public ConsumerHandle(IModel ch, string tag, CancellationToken token)
        {
            _ch = ch;
            _tag = tag;
            if (token.CanBeCanceled)
                token.Register(() =>
                {
                    try { _ch.BasicCancel(_tag); } catch { /* ignore */ }
                });
        }
        public void Dispose()
        {
            try { _ch.BasicCancel(_tag); } catch { /* ignore */ }
        }
    }

    private sealed class NullDisposable : IDisposable
    {
        public static readonly NullDisposable Instance = new();
        public void Dispose() { }
    }
}

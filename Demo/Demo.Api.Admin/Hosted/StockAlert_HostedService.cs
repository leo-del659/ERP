using System.Text.Json;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;

namespace Demo.Api.Admin.Hosted;

/// <summary>
/// 库存预警消费者。
/// 通过 RabbitMQ 消费 erp.stock.alert 队列，消费端以 MessageId 为键做 Redis 幂等去重，
/// 并将「物料+仓库」的最新预警快照写入 Redis 缓存。
/// </summary>
public class StockAlert_HostedService : BackgroundService
{
    private readonly RabbitMq_Bus _bus;
    private readonly IServiceScopeFactory _scope;
    private readonly Redis_Cache _redis;
    private readonly ILogger<StockAlert_HostedService> _log;
    private static readonly TimeSpan IdemTtl = TimeSpan.FromHours(24);
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(30);

    public StockAlert_HostedService(RabbitMq_Bus bus, IServiceScopeFactory scope, Redis_Cache redis, ILogger<StockAlert_HostedService> log)
    {
        _bus = bus;
        _scope = scope;
        _redis = redis;
        _log = log;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_bus.Enabled)
        {
            _log.LogWarning("RabbitMQ 未配置/连接失败，库存预警消费者未启动（预警仅落操作日志）");
            return Task.CompletedTask;
        }

        _bus.Consume(RabbitMq_Bus.StockAlertQueue, HandleAsync, stoppingToken);
        return Task.CompletedTask;
    }

    private async Task HandleAsync(string json, CancellationToken ct)
    {
        Stock_Alert_Message? msg;
        try
        {
            msg = JsonSerializer.Deserialize<Stock_Alert_Message>(json);
        }
        catch (JsonException ex)
        {
            
            _log.LogError(ex, "库存预警消息反序列化失败，视为毒消息 json={Json}", json);
            throw;
        }

        if (msg == null)
            throw new InvalidOperationException("收到空库存预警消息");

        // 幂等：同一 MessageId 仅处理一次（Redis SET NX）
        var msgId = msg.MessageId ?? $"{msg.MaterialId}:{msg.WarehouseId}:{msg.CurrentQty}";
        var idemKey = $"stock:alert:idem:{msgId}";
        if (!await _redis.TryLockAsync(idemKey, IdemTtl))
        {
            _log.LogInformation("库存预警重复消息，已跳过 msgId={MsgId}", msgId);
            return;
        }

        using var sp = _scope.CreateScope();
        var db = sp.ServiceProvider.GetRequiredService<ErpDbContext>();

        // 缓存最新预警快照（看板/接口可免 DB 读取）
        var cacheKey = $"stock:alert:latest:{msg.MaterialId}:{msg.WarehouseId}";
        var snapshot = new
        {
            msg.MaterialId,
            msg.MaterialCode,
            msg.MaterialName,
            msg.WarehouseId,
            msg.CurrentQty,
            msg.SafetyStock,
            AlertTime = DateTime.Now
        };
        await _redis.SetAsync(cacheKey, snapshot, CacheTtl);

        db.RptKpis.Add(new RptKpi
        {
            KpiCode = "STOCK_ALERT",
            KpiName = $"{msg.MaterialCode} 低于安全库存",
            KpiValue = msg.CurrentQty,
            KpiUnit = "qty",
            CalcDate = DateOnly.FromDateTime(DateTime.Today),
            UpdateTime = DateTime.Now
        });
        db.SysOperLogs.Add(new SysOperLog
        {
            Module = "库存预警",
            Action = "ALERT",
            Detail = $"{msg.MaterialCode} 仓库{msg.WarehouseId} 现存量{msg.CurrentQty} 安全库存{msg.SafetyStock}",
            OperTime = DateTime.Now
        });

        await db.SaveChangesAsync(ct);

        _log.LogWarning("库存预警已处理 {Code} qty={Qty} safety={Safety}", msg.MaterialCode, msg.CurrentQty, msg.SafetyStock);
    }
}

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 支付回调幂等记录
/// </summary>
public partial class PayNotify
{
    public long Id { get; set; }

    public string NotifyNo { get; set; } = null!;

    public string? Channel { get; set; }

    public string? BizNo { get; set; }

    public long? ReceivableId { get; set; }

    public decimal Amount { get; set; }

    public sbyte Status { get; set; }

    public string? Payload { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }
}

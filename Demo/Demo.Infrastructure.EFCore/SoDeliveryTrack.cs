namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 发货物流跟踪记录：每次物流状态变更留痕，形成发货单的时间线。
/// </summary>
public partial class SoDeliveryTrack
{
    public long Id { get; set; }

    /// <summary>发货单ID</summary>
    public long DeliveryId { get; set; }

    /// <summary>承运商</summary>
    public string? Carrier { get; set; }

    /// <summary>运单号</summary>
    public string? TrackNo { get; set; }

    /// <summary>状态：已揽收/运输中/派送中/已签收/异常</summary>
    public string? TrackStatus { get; set; }

    /// <summary>当前位置</summary>
    public string? Location { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    public DateTime TrackTime { get; set; }
}

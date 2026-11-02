using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 应收单
/// </summary>
public partial class FinReceivable
{
    public long Id { get; set; }

    /// <summary>
    /// 应收单号
    /// </summary>
    public string RecvNo { get; set; } = null!;

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 来源销售订单ID
    /// </summary>
    public long? SourceOrderId { get; set; }

    /// <summary>
    /// 应收金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 已收金额
    /// </summary>
    public decimal? ReceivedAmount { get; set; }

    /// <summary>
    /// 到期日
    /// </summary>
    public DateOnly? DueDate { get; set; }

    /// <summary>
    /// 0未收/1部分收/2已收
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

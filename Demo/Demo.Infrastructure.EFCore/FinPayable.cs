using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 应付单
/// </summary>
public partial class FinPayable
{
    public long Id { get; set; }

    /// <summary>
    /// 应付单号
    /// </summary>
    public string PayNo { get; set; } = null!;

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 来源采购订单ID
    /// </summary>
    public long? SourceOrderId { get; set; }

    /// <summary>
    /// 应付金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 已付金额
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 到期日
    /// </summary>
    public DateOnly? DueDate { get; set; }

    /// <summary>
    /// 0未付/1部分付/2已付
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

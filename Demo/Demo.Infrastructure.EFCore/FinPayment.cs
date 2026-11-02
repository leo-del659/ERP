using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 付款单
/// </summary>
public partial class FinPayment
{
    public long Id { get; set; }

    /// <summary>
    /// 付款单号
    /// </summary>
    public string PaymentNo { get; set; } = null!;

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 核销应付单ID
    /// </summary>
    public long? PayableId { get; set; }

    /// <summary>
    /// 付款金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 付款日期
    /// </summary>
    public DateOnly PaymentDate { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    public DateTime? CreateTime { get; set; }
}

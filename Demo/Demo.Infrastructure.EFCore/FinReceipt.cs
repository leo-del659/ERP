using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 收款单
/// </summary>
public partial class FinReceipt
{
    public long Id { get; set; }

    /// <summary>
    /// 收款单号
    /// </summary>
    public string ReceiptNo { get; set; } = null!;

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 核销应收单ID
    /// </summary>
    public long? ReceivableId { get; set; }

    /// <summary>
    /// 收款金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 收款日期
    /// </summary>
    public DateOnly ReceiptDate { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    public DateTime? CreateTime { get; set; }
}

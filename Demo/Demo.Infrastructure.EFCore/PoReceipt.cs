using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 采购收货单
/// </summary>
public partial class PoReceipt
{
    public long Id { get; set; }

    /// <summary>
    /// 收货单号
    /// </summary>
    public string ReceiptNo { get; set; } = null!;

    /// <summary>
    /// 采购订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 收货日期
    /// </summary>
    public DateOnly ReceiptDate { get; set; }

    /// <summary>
    /// 0待检/1合格/2不合格
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    public DateTime? CreateTime { get; set; }
}

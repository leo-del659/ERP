using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 采购订单主表
/// </summary>
public partial class PoOrder
{
    public long Id { get; set; }

    /// <summary>
    /// 采购订单号
    /// </summary>
    public string OrderNo { get; set; } = null!;

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 下单日期
    /// </summary>
    public DateOnly OrderDate { get; set; }

    /// <summary>
    /// 0待审/1已审/2部分收货/3已完成
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回（采购订单变更审批回写）</summary>
    public sbyte? AuditStatus { get; set; }

    /// <summary>
    /// 订单总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }
}

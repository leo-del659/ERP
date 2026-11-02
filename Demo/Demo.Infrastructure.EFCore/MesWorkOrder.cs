using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 生产工单
/// </summary>
public partial class MesWorkOrder
{
    /// <summary>主键</summary>
    public long Id { get; set; }

    /// <summary>工单号</summary>
    public string OrderNo { get; set; } = null!;

    /// <summary>产品/成品ID</summary>
    public long ProductId { get; set; }

    /// <summary>计划数量</summary>
    public int PlanQty { get; set; }

    /// <summary>已产数量</summary>
    public int DoneQty { get; set; }

    public sbyte Priority { get; set; }

    public sbyte Status { get; set; }

    /// <summary>计划开工</summary>
    public DateOnly? PlanStart { get; set; }

    /// <summary>计划完工</summary>
    public DateOnly? PlanEnd { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    /// <summary>关联生产订单ID(pr_order.id)</summary>
    public long? PrOrderId { get; set; }

    public sbyte Source { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

}
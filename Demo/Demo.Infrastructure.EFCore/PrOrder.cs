using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 生产订单
/// </summary>
public partial class PrOrder
{
    public long Id { get; set; }

    /// <summary>
    /// 生产制令号
    /// </summary>
    public string OrderNo { get; set; } = null!;

    /// <summary>
    /// 要生产的成品ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 计划生产数量
    /// </summary>
    public decimal PlanQty { get; set; }

    /// <summary>
    /// 已完工入库数量
    /// </summary>
    public decimal? CompletedQty { get; set; }

    /// <summary>
    /// 计划开工日
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// 计划完工日
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// 0计划/1下达/2生产中/3完工/4关闭
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>
    /// 来源销售订单ID
    /// </summary>
    public long? SourceSoId { get; set; }

    /// <summary>
    /// 来源MPS ID
    /// </summary>
    public long? SourceMpsId { get; set; }

    public DateTime? CreateTime { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 主生产计划MPS
/// </summary>
public partial class PrMp
{
    public long Id { get; set; }

    /// <summary>
    /// MPS计划号
    /// </summary>
    public string MpsNo { get; set; } = null!;

    /// <summary>
    /// 成品物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal PlanQty { get; set; }

    /// <summary>
    /// 计划开工日
    /// </summary>
    public DateOnly? PlanStartDate { get; set; }

    /// <summary>
    /// 计划完工日
    /// </summary>
    public DateOnly? PlanEndDate { get; set; }

    /// <summary>
    /// 来源：SO销售/FCST预测/STOCK安全库存
    /// </summary>
    public string? SourceType { get; set; }

    /// <summary>
    /// 来源单据ID
    /// </summary>
    public long? SourceId { get; set; }

    /// <summary>
    /// 0草稿/1确认/2已转生产订单
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

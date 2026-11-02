using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 成本计算单
/// </summary>
public partial class FinCost
{
    public long Id { get; set; }

    /// <summary>
    /// 成本单号
    /// </summary>
    public string CostNo { get; set; } = null!;

    /// <summary>
    /// 生产订单ID
    /// </summary>
    public long? PrOrderId { get; set; }

    /// <summary>
    /// 成品物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 材料成本
    /// </summary>
    public decimal? MaterialCost { get; set; }

    /// <summary>
    /// 人工成本
    /// </summary>
    public decimal? LaborCost { get; set; }

    /// <summary>
    /// 制造费用
    /// </summary>
    public decimal? OverheadCost { get; set; }

    /// <summary>
    /// 总成本
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateOnly? CalcDate { get; set; }

    public DateTime? CreateTime { get; set; }
}

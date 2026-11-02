using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 看板指标
/// </summary>
public partial class RptKpi
{
    public long Id { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    public string KpiCode { get; set; } = null!;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string KpiName { get; set; } = null!;

    /// <summary>
    /// 指标值
    /// </summary>
    public decimal? KpiValue { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string? KpiUnit { get; set; }

    /// <summary>
    /// 统计日期
    /// </summary>
    public DateOnly? CalcDate { get; set; }

    public DateTime? UpdateTime { get; set; }
}

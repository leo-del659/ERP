using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 考核指标库
/// </summary>
public partial class HrPerfIndicator
{
    public long Id { get; set; }

    /// <summary>指标编码</summary>
    public string IndNo { get; set; } = null!;

    /// <summary>指标名称</summary>
    public string IndName { get; set; } = null!;

    /// <summary>1业绩/2能力/3态度</summary>
    public sbyte? Category { get; set; }

    /// <summary>权重%</summary>
    public decimal? Weight { get; set; }

    /// <summary>目标值</summary>
    public string? Target { get; set; }

    public DateTime? CreateTime { get; set; }

}
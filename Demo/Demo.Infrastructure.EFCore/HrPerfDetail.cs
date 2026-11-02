using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 考核明细（按指标）
/// </summary>
public partial class HrPerfDetail
{
    public long Id { get; set; }

    /// <summary>考核结果ID</summary>
    public long ResultId { get; set; }

    /// <summary>指标ID</summary>
    public long? IndId { get; set; }

    public string? IndName { get; set; }

    /// <summary>指标得分</summary>
    public decimal? Score { get; set; }

    /// <summary>权重%</summary>
    public decimal? Weight { get; set; }

    public DateTime? CreateTime { get; set; }

}
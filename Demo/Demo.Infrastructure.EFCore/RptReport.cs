using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 报表定义
/// </summary>
public partial class RptReport
{
    public long Id { get; set; }

    /// <summary>
    /// 报表编码
    /// </summary>
    public string ReportCode { get; set; } = null!;

    /// <summary>
    /// 报表名称
    /// </summary>
    public string ReportName { get; set; } = null!;

    /// <summary>
    /// 类型：TABLE/CHART
    /// </summary>
    public string? ReportType { get; set; }

    /// <summary>
    /// 取数SQL
    /// </summary>
    public string? SqlText { get; set; }

    /// <summary>
    /// 1启用/0停用
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

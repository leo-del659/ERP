using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 生产报工
/// </summary>
public partial class PrReport
{
    public long Id { get; set; }

    /// <summary>
    /// 生产订单ID
    /// </summary>
    public long PrOrderId { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; }

    /// <summary>
    /// 报工数量
    /// </summary>
    public decimal ReportQty { get; set; }

    /// <summary>
    /// 良品数量
    /// </summary>
    public decimal? GoodQty { get; set; }

    /// <summary>
    /// 不良品数量
    /// </summary>
    public decimal? BadQty { get; set; }

    /// <summary>
    /// 工时
    /// </summary>
    public decimal? WorkHours { get; set; }

    /// <summary>
    /// 报工时间
    /// </summary>
    public DateTime? ReportTime { get; set; }
}

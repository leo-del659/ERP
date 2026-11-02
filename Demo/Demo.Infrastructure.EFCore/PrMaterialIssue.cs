using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 生产领料
/// </summary>
public partial class PrMaterialIssue
{
    public long Id { get; set; }

    /// <summary>
    /// 生产订单ID
    /// </summary>
    public long PrOrderId { get; set; }

    /// <summary>
    /// 领用物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 领用数量
    /// </summary>
    public decimal IssueQty { get; set; }

    /// <summary>
    /// 领用时间
    /// </summary>
    public DateTime? IssueTime { get; set; }
}

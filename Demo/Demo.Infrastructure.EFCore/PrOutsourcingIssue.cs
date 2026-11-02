using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 委外发料
/// </summary>
public partial class PrOutsourcingIssue
{
    public long Id { get; set; }

    /// <summary>
    /// 委外订单ID
    /// </summary>
    public long OutsourceId { get; set; }

    /// <summary>
    /// 发料物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 发料数量
    /// </summary>
    public decimal IssueQty { get; set; }

    public DateTime? IssueTime { get; set; }
}

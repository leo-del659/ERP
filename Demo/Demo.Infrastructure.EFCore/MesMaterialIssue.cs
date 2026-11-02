using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 领料单
/// </summary>
public partial class MesMaterialIssue
{
    /// <summary>主键</summary>
    public long Id { get; set; }

    /// <summary>领料单号</summary>
    public string IssueNo { get; set; } = null!;

    /// <summary>工单ID</summary>
    public long WorkOrderId { get; set; }

    /// <summary>物料ID</summary>
    public long MaterialId { get; set; }

    /// <summary>领用数量</summary>
    public decimal Qty { get; set; }

    public sbyte Status { get; set; }

    /// <summary>领料时间</summary>
    public DateTime? IssueTime { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

}
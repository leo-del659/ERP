using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 岗位编制控制
/// </summary>
public partial class HrPositionQuota
{
    public long Id { get; set; }

    /// <summary>部门ID</summary>
    public long DeptId { get; set; }

    /// <summary>岗位ID</summary>
    public long PosId { get; set; }

    /// <summary>编制数</summary>
    public int? PlanCount { get; set; }

    /// <summary>在岗数</summary>
    public int? ActualCount { get; set; }

    /// <summary>冻结/预留数</summary>
    public int? FrozenCount { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateTime { get; set; }

}
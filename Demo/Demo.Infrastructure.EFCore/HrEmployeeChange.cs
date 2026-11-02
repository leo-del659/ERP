using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 人事异动轨迹（全周期）
/// </summary>
public partial class HrEmployeeChange
{
    public long Id { get; set; }

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>1入职/2转正/3调动/4晋升/5降职/6离职/7复职</summary>
    public sbyte ChangeType { get; set; }

    public long? OldDeptId { get; set; }

    public long? NewDeptId { get; set; }

    public long? OldPosId { get; set; }

    public long? NewPosId { get; set; }

    public sbyte? OldStatus { get; set; }

    public sbyte? NewStatus { get; set; }

    /// <summary>生效日期</summary>
    public DateOnly? EffectDate { get; set; }

    public string? Reason { get; set; }

    /// <summary>关联 sys_approval.id（HR_EMP）</summary>
    public long? ApprovalId { get; set; }

    /// <summary>操作人</summary>
    public string? Operator { get; set; }

    public DateTime? CreateTime { get; set; }

}
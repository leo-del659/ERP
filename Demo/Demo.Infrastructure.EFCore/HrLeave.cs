using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 请假单
/// </summary>
public partial class HrLeave
{
    public long Id { get; set; }

    /// <summary>请假单号</summary>
    public string LeaveNo { get; set; } = null!;

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>1年假/2事假/3病假/4调休/5婚假/6产假</summary>
    public sbyte? LeaveType { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    /// <summary>请假天数</summary>
    public decimal? Days { get; set; }

    public string? Reason { get; set; }

    /// <summary>0草稿/1审批中/2通过/3驳回</summary>
    public sbyte? Status { get; set; }

    /// <summary>关联 sys_approval.id（HR_LEAVE）</summary>
    public long? ApprovalId { get; set; }

    public DateTime? CreateTime { get; set; }

}
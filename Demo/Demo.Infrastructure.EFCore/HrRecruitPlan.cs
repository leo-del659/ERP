using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 人力需求/招聘计划
/// </summary>
public partial class HrRecruitPlan
{
    public long Id { get; set; }

    /// <summary>招聘计划(人力需求)单号</summary>
    public string PlanNo { get; set; } = null!;

    /// <summary>需求部门</summary>
    public long DeptId { get; set; }

    /// <summary>需求岗位</summary>
    public long PosId { get; set; }

    /// <summary>需求人数</summary>
    public int? NeedCount { get; set; }

    /// <summary>需求理由</summary>
    public string? Reason { get; set; }

    /// <summary>0草稿/1审批中/2通过/3关闭</summary>
    public sbyte? Status { get; set; }

    /// <summary>关联 sys_approval.id（HR_RECRUIT）</summary>
    public long? ApprovalId { get; set; }

    public DateTime? CreateTime { get; set; }

}
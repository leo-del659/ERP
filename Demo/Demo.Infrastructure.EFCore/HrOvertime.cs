using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 加班单
/// </summary>
public partial class HrOvertime
{
    public long Id { get; set; }

    /// <summary>加班单号</summary>
    public string OtNo { get; set; } = null!;

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>加班日期</summary>
    public DateOnly? OtDate { get; set; }

    /// <summary>加班小时</summary>
    public decimal? Hours { get; set; }

    public string? Reason { get; set; }

    /// <summary>0草稿/1审批中/2通过/3驳回</summary>
    public sbyte? Status { get; set; }

    /// <summary>关联 sys_approval.id（HR_OVERTIME）</summary>
    public long? ApprovalId { get; set; }

    public DateTime? CreateTime { get; set; }

}
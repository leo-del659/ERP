using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 劳动合同（含到期预警：按 end_date 查询）
/// </summary>
public partial class HrContract
{
    public long Id { get; set; }

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>合同编号</summary>
    public string ContractNo { get; set; } = null!;

    /// <summary>1固定期限/2无固定/3实习/4劳务</summary>
    public sbyte? ContractType { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    /// <summary>签订日期</summary>
    public DateOnly? SignDate { get; set; }

    /// <summary>试用期(月)</summary>
    public int? ProbationMonths { get; set; }

    /// <summary>合同约定薪资</summary>
    public decimal? Salary { get; set; }

    /// <summary>0草稿/1生效/2续签/3变更/4解除/5到期</summary>
    public sbyte? Status { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateTime { get; set; }

}
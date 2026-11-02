using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 考核结果（应用至薪资/晋升）
/// </summary>
public partial class HrPerfResult
{
    public long Id { get; set; }

    /// <summary>考核计划ID</summary>
    public long PlanId { get; set; }

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>综合得分</summary>
    public decimal? Score { get; set; }

    /// <summary>1优秀/2良好/3合格/4待改进</summary>
    public sbyte? Level { get; set; }

    /// <summary>考核结论</summary>
    public string? Conclusion { get; set; }

    /// <summary>结果是否应用于薪资(0否/1是)</summary>
    public sbyte? ApplySalary { get; set; }

    /// <summary>结果是否应用于晋升(0否/1是)</summary>
    public sbyte? ApplyPromote { get; set; }

    public DateTime? CreateTime { get; set; }

}
using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 培训记录
/// </summary>
public partial class HrTrainRecord
{
    public long Id { get; set; }

    /// <summary>培训计划ID</summary>
    public long PlanId { get; set; }

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>培训课时</summary>
    public decimal? Hours { get; set; }

    /// <summary>考核成绩</summary>
    public decimal? Score { get; set; }

    /// <summary>获得证书</summary>
    public string? Certificate { get; set; }

    public DateTime? CreateTime { get; set; }

}
using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 员工福利
/// </summary>
public partial class HrWelfare
{
    public long Id { get; set; }

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>福利类型：节日/餐补/交通/体检</summary>
    public string? WelfareType { get; set; }

    public decimal? Amount { get; set; }

    /// <summary>所属月份(YYYY-MM)</summary>
    public string? Month { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateTime { get; set; }

}
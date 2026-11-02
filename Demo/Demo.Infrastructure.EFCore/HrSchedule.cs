using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 排班表
/// </summary>
public partial class HrSchedule
{
    public long Id { get; set; }

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>班次ID</summary>
    public long ShiftId { get; set; }

    /// <summary>排班日期</summary>
    public DateOnly? WorkDate { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateTime { get; set; }

}
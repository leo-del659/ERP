using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 打卡/考勤明细（结果传递至薪资核算）
/// </summary>
public partial class HrAttendance
{
    public long Id { get; set; }

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>排班ID</summary>
    public long? ScheduleId { get; set; }

    public DateOnly? WorkDate { get; set; }

    /// <summary>上班打卡</summary>
    public DateTime? PunchIn { get; set; }

    /// <summary>下班打卡</summary>
    public DateTime? PunchOut { get; set; }

    /// <summary>迟到分钟</summary>
    public int? LateMin { get; set; }

    /// <summary>早退分钟</summary>
    public int? EarlyMin { get; set; }

    /// <summary>0否/1是</summary>
    public sbyte? Absent { get; set; }

    /// <summary>加班分钟</summary>
    public int? OvertimeMin { get; set; }

    /// <summary>0正常/1迟到/2早退/3缺勤/4加班</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
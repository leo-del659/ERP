using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 班次定义
/// </summary>
public partial class HrShift
{
    public long Id { get; set; }

    /// <summary>班次编码</summary>
    public string ShiftNo { get; set; } = null!;

    /// <summary>班次名称</summary>
    public string ShiftName { get; set; } = null!;

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    /// <summary>0否/1跨天</summary>
    public sbyte? CrossDay { get; set; }

    /// <summary>休息分钟</summary>
    public int? RestMinutes { get; set; }

    /// <summary>0停用/1启用</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
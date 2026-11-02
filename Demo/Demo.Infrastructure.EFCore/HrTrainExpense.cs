using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 培训费用
/// </summary>
public partial class HrTrainExpense
{
    public long Id { get; set; }

    /// <summary>培训计划ID</summary>
    public long PlanId { get; set; }

    /// <summary>费用类型：讲师费/场地/教材/差旅</summary>
    public string? ExpenseType { get; set; }

    public decimal? Amount { get; set; }

    /// <summary>发生日期</summary>
    public DateOnly? OccurDate { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateTime { get; set; }

}
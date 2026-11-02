using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 考核计划
/// </summary>
public partial class HrPerfPlan
{
    public long Id { get; set; }

    /// <summary>考核计划单号</summary>
    public string PlanNo { get; set; } = null!;

    /// <summary>计划名称</summary>
    public string PlanName { get; set; } = null!;

    /// <summary>考核期间(YYYY-Qx / YYYY-MM)</summary>
    public string? Period { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    /// <summary>0草稿/1进行中/2已完成</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
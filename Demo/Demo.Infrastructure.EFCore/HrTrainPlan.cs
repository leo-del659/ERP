using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 培训计划
/// </summary>
public partial class HrTrainPlan
{
    public long Id { get; set; }

    /// <summary>培训计划单号</summary>
    public string PlanNo { get; set; } = null!;

    /// <summary>培训名称</summary>
    public string PlanName { get; set; } = null!;

    /// <summary>类型：新员工/技能/安全/管理</summary>
    public string? TrainType { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    /// <summary>讲师</summary>
    public string? Trainer { get; set; }

    /// <summary>培训预算</summary>
    public decimal? Budget { get; set; }

    /// <summary>0草稿/1执行中/2完成</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
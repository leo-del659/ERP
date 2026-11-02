using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 工艺工序
/// </summary>
public partial class MesRoutingStep
{
    /// <summary>主键</summary>
    public long Id { get; set; }

    /// <summary>工艺路线ID</summary>
    public long RoutingId { get; set; }

    /// <summary>工序号</summary>
    public int StepNo { get; set; }

    /// <summary>工序名称</summary>
    public string StepName { get; set; } = null!;

    /// <summary>工位ID</summary>
    public long? WorkstationId { get; set; }

    /// <summary>标准工时</summary>
    public decimal StdHours { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

}
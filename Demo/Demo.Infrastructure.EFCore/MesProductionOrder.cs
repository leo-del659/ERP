using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 生产任务(派工)
/// </summary>
public partial class MesProductionOrder
{
    /// <summary>主键</summary>
    public long Id { get; set; }

    /// <summary>生产任务号</summary>
    public string ProdNo { get; set; } = null!;

    /// <summary>所属工单ID</summary>
    public long WorkOrderId { get; set; }

    /// <summary>工位ID</summary>
    public long? WorkstationId { get; set; }

    /// <summary>计划数量</summary>
    public int PlanQty { get; set; }

    /// <summary>已产数量</summary>
    public int DoneQty { get; set; }

    public sbyte Status { get; set; }

    /// <summary>开工时间</summary>
    public DateTime? StartTime { get; set; }

    /// <summary>完工时间</summary>
    public DateTime? EndTime { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

}
using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 报工单
/// </summary>
public partial class MesWorkReport
{
    /// <summary>主键</summary>
    public long Id { get; set; }

    /// <summary>报工单号</summary>
    public string ReportNo { get; set; } = null!;

    /// <summary>生产任务ID</summary>
    public long ProductionOrderId { get; set; }

    /// <summary>操作工ID</summary>
    public long? EmployeeId { get; set; }

    /// <summary>合格数</summary>
    public int GoodQty { get; set; }

    /// <summary>不良数</summary>
    public int BadQty { get; set; }

    /// <summary>工时</summary>
    public decimal WorkHours { get; set; }

    public sbyte Status { get; set; }

    /// <summary>报工时间</summary>
    public DateTime? ReportTime { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

}
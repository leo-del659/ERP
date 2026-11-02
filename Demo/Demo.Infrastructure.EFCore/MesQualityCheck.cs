using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 质量检验
/// </summary>
public partial class MesQualityCheck
{
    /// <summary>主键</summary>
    public long Id { get; set; }

    /// <summary>质检单号</summary>
    public string CheckNo { get; set; } = null!;

    /// <summary>生产任务ID</summary>
    public long? ProductionOrderId { get; set; }

    /// <summary>报工单ID</summary>
    public long? WorkReportId { get; set; }

    public sbyte CheckType { get; set; }

    /// <summary>检验数量</summary>
    public int Qty { get; set; }

    /// <summary>合格数</summary>
    public int PassQty { get; set; }

    /// <summary>不良数</summary>
    public int FailQty { get; set; }

    public sbyte Result { get; set; }

    /// <summary>检验时间</summary>
    public DateTime? CheckTime { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

}
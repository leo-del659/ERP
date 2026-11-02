using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 出库单
/// </summary>
public partial class InvOutOrder
{
    public long Id { get; set; }

    /// <summary>
    /// 出库单号
    /// </summary>
    public string OutNo { get; set; } = null!;

    /// <summary>
    /// 类型：SO销售/PR领料/OTHER其他
    /// </summary>
    public string OutType { get; set; } = null!;

    /// <summary>
    /// 来源单据号
    /// </summary>
    public string? SourceNo { get; set; }

    /// <summary>
    /// 仓库ID
    /// </summary>
    public long WarehouseId { get; set; }

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateOnly OutDate { get; set; }

    /// <summary>
    /// 0待审/1已审
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    public DateTime? CreateTime { get; set; }
}

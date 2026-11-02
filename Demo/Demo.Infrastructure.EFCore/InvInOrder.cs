using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 入库单
/// </summary>
public partial class InvInOrder
{
    public long Id { get; set; }

    /// <summary>
    /// 入库单号
    /// </summary>
    public string InNo { get; set; } = null!;

    /// <summary>
    /// 类型：PO采购/PR生产/OTHER其他
    /// </summary>
    public string InType { get; set; } = null!;

    /// <summary>
    /// 来源单据号
    /// </summary>
    public string? SourceNo { get; set; }

    /// <summary>
    /// 仓库ID
    /// </summary>
    public long WarehouseId { get; set; }

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateOnly InDate { get; set; }

    /// <summary>
    /// 0待审/1已审
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    public DateTime? CreateTime { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 盘点单
/// </summary>
public partial class InvCheck
{
    public long Id { get; set; }

    /// <summary>
    /// 盘点单号
    /// </summary>
    public string CheckNo { get; set; } = null!;

    /// <summary>
    /// 仓库ID
    /// </summary>
    public long WarehouseId { get; set; }

    /// <summary>
    /// 盘点日期
    /// </summary>
    public DateOnly CheckDate { get; set; }

    /// <summary>
    /// 0盘点中/1已完成
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }
}

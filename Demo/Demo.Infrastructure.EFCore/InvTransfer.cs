using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 调拨单
/// </summary>
public partial class InvTransfer
{
    public long Id { get; set; }

    /// <summary>
    /// 调拨单号
    /// </summary>
    public string TransferNo { get; set; } = null!;

    /// <summary>
    /// 调出仓库ID
    /// </summary>
    public long FromWarehouseId { get; set; }

    /// <summary>
    /// 调入仓库ID
    /// </summary>
    public long ToWarehouseId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 调拨数量
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 调拨日期
    /// </summary>
    public DateOnly TransferDate { get; set; }

    /// <summary>
    /// 0待审/1已审
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }
}

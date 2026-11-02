using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 库存流水
/// </summary>
public partial class InvTransaction
{
    public long Id { get; set; }

    /// <summary>
    /// 流水单号
    /// </summary>
    public string TransNo { get; set; } = null!;

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 仓库ID
    /// </summary>
    public long WarehouseId { get; set; }

    /// <summary>
    /// IN_PO/OUT_SO/IN_PR/OUT_PR/ADJ
    /// </summary>
    public string TransType { get; set; } = null!;

    /// <summary>
    /// 1入库/-1出库
    /// </summary>
    public sbyte Direction { get; set; }

    /// <summary>
    /// 变动数量（正数）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 变动前数量
    /// </summary>
    public decimal? BeforeQty { get; set; }

    /// <summary>
    /// 变动后数量
    /// </summary>
    public decimal? AfterQty { get; set; }

    /// <summary>
    /// 来源单据号
    /// </summary>
    public string? SourceNo { get; set; }

    public DateTime? TransTime { get; set; }
}

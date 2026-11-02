using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 采购收货明细
/// </summary>
public partial class PoReceiptDetail
{
    public long Id { get; set; }

    /// <summary>
    /// 收货单ID
    /// </summary>
    public long ReceiptId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 收货数量
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 合格数量
    /// </summary>
    public decimal? QualifiedQty { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 采购退货单
/// </summary>
public partial class PoReturn
{
    public long Id { get; set; }

    /// <summary>
    /// 退货单号
    /// </summary>
    public string ReturnNo { get; set; } = null!;

    /// <summary>
    /// 来源收货单ID
    /// </summary>
    public long? ReceiptId { get; set; }

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 退货数量
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 退货日期
    /// </summary>
    public DateOnly ReturnDate { get; set; }
}

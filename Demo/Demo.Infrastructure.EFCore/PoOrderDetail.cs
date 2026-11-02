using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 采购订单明细
/// </summary>
public partial class PoOrderDetail
{
    public long Id { get; set; }

    /// <summary>
    /// 采购订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 采购数量
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 含税单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 已收货数量
    /// </summary>
    public decimal? ReceivedQty { get; set; }
}

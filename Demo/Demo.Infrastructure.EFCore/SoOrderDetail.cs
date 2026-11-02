using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 销售订单明细
/// </summary>
public partial class SoOrderDetail
{
    public long Id { get; set; }

    /// <summary>
    /// 销售订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 销售数量
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 已发货数量
    /// </summary>
    public decimal? ShippedQty { get; set; }
}

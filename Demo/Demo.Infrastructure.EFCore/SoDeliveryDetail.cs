using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 发货明细
/// </summary>
public partial class SoDeliveryDetail
{
    public long Id { get; set; }

    /// <summary>
    /// 发货单ID
    /// </summary>
    public long DeliveryId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 发货数量
    /// </summary>
    public decimal Quantity { get; set; }
}

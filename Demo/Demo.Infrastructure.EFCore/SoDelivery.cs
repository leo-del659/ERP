using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 发货单
/// </summary>
public partial class SoDelivery
{
    public long Id { get; set; }

    /// <summary>
    /// 发货单号
    /// </summary>
    public string DeliveryNo { get; set; } = null!;

    /// <summary>
    /// 销售订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 发货日期
    /// </summary>
    public DateOnly DeliveryDate { get; set; }

    /// <summary>
    /// 0待发/1已发/2已签收
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

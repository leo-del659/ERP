using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 销售退货单
/// </summary>
public partial class SoReturn
{
    public long Id { get; set; }

    /// <summary>
    /// 退货单号
    /// </summary>
    public string ReturnNo { get; set; } = null!;

    /// <summary>
    /// 来源发货单ID
    /// </summary>
    public long? DeliveryId { get; set; }

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

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

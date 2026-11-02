using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 销售订单主表
/// </summary>
public partial class SoOrder
{
    public long Id { get; set; }

    /// <summary>
    /// 销售订单号
    /// </summary>
    public string OrderNo { get; set; } = null!;

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateOnly OrderDate { get; set; }

    /// <summary>
    /// 要求交货日期
    /// </summary>
    public DateOnly? DeliveryDate { get; set; }

    /// <summary>
    /// 0新建/1已确认/2已发货/3已完成
    /// </summary>
    public sbyte? Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 委外订单
/// </summary>
public partial class PrOutsourcing
{
    public long Id { get; set; }

    /// <summary>
    /// 委外订单号
    /// </summary>
    public string OutsourceNo { get; set; } = null!;

    /// <summary>
    /// 加工商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 委外成品ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 委外数量
    /// </summary>
    public decimal PlanQty { get; set; }

    /// <summary>
    /// 已回收数量
    /// </summary>
    public decimal? ReceivedQty { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// 0计划/1外发/2部分回收/3完成
    /// </summary>
    public sbyte? Status { get; set; }
}

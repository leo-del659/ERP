using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 供应商报价
/// </summary>
public partial class PoSupplierQuote
{
    public long Id { get; set; }

    /// <summary>
    /// 供应商ID
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 报价单价
    /// </summary>
    public decimal QuotePrice { get; set; }

    /// <summary>
    /// 有效期起
    /// </summary>
    public DateOnly? ValidFrom { get; set; }

    /// <summary>
    /// 有效期止
    /// </summary>
    public DateOnly? ValidTo { get; set; }

    public DateTime? CreateTime { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// MRP运算结果
/// </summary>
public partial class PrMrp
{
    public long Id { get; set; }

    /// <summary>
    /// MRP运算号
    /// </summary>
    public string MrpNo { get; set; } = null!;

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 毛需求
    /// </summary>
    public decimal? GrossRequirement { get; set; }

    /// <summary>
    /// 现有库存
    /// </summary>
    public decimal? OnHandQty { get; set; }

    /// <summary>
    /// 在途量
    /// </summary>
    public decimal? OnOrderQty { get; set; }

    /// <summary>
    /// 安全库存
    /// </summary>
    public decimal? SafetyStock { get; set; }

    /// <summary>
    /// 净需求
    /// </summary>
    public decimal? NetRequirement { get; set; }

    /// <summary>
    /// 建议类型：BUY采购/MAKE自制
    /// </summary>
    public string? SuggestType { get; set; }

    /// <summary>
    /// 建议数量
    /// </summary>
    public decimal? SuggestQty { get; set; }

    /// <summary>
    /// 建议日期
    /// </summary>
    public DateOnly? SuggestDate { get; set; }

    /// <summary>
    /// 0未处理/1已转单
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

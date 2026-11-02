using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// BOM物料清单
/// </summary>
public partial class MdBom
{
    /// <summary>
    /// BOM ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 父项物料ID（成品/半成品）
    /// </summary>
    public long ParentMaterialId { get; set; }

    /// <summary>
    /// 子项物料ID（原材料/半成品）
    /// </summary>
    public long ChildMaterialId { get; set; }

    /// <summary>
    /// 单位用量
    /// </summary>
    public decimal QtyPerUnit { get; set; }

    /// <summary>
    /// 损耗率（0.02=2%）
    /// </summary>
    public decimal? LossRate { get; set; }

    /// <summary>
    /// BOM版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// 状态：1启用/0停用
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

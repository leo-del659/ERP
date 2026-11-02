using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 物料主数据
/// </summary>
public partial class MdMaterial
{
    /// <summary>
    /// 物料ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = null!;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = null!;

    /// <summary>
    /// 规格型号
    /// </summary>
    public string? Spec { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// 物料分类：成品/半成品/原材料/辅料
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// 安全库存
    /// </summary>
    public decimal? SafetyStock { get; set; }

    /// <summary>
    /// 状态：1启用/0停用
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

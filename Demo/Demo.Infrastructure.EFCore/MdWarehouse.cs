using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 仓库档案
/// </summary>
public partial class MdWarehouse
{
    /// <summary>
    /// 仓库ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string WarehouseCode { get; set; } = null!;

    /// <summary>
    /// 仓库名称
    /// </summary>
    public string WarehouseName { get; set; } = null!;

    /// <summary>
    /// 仓库位置
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// 状态：1启用/0停用
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

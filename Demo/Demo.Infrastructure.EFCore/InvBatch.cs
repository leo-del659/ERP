using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 批次库存
/// </summary>
public partial class InvBatch
{
    public long Id { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 仓库ID
    /// </summary>
    public long WarehouseId { get; set; }

    /// <summary>
    /// 批次号
    /// </summary>
    public string BatchNo { get; set; } = null!;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateOnly? ProductionDate { get; set; }

    /// <summary>
    /// 有效期
    /// </summary>
    public DateOnly? ExpiryDate { get; set; }
}

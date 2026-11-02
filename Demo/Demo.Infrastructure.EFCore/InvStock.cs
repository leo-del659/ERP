using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 库存实时台账
/// </summary>
public partial class InvStock
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
    /// 当前库存数量
    /// </summary>
    public decimal CurrentQty { get; set; }

    /// <summary>
    /// 锁定库存
    /// </summary>
    public decimal? LockedQty { get; set; }

    public DateTime? LastUpdate { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 出库明细
/// </summary>
public partial class InvOutDetail
{
    public long Id { get; set; }

    /// <summary>
    /// 出库单ID
    /// </summary>
    public long OutOrderId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 出库数量
    /// </summary>
    public decimal Quantity { get; set; }
}

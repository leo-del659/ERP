using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 入库明细
/// </summary>
public partial class InvInDetail
{
    public long Id { get; set; }

    /// <summary>
    /// 入库单ID
    /// </summary>
    public long InOrderId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 入库数量
    /// </summary>
    public decimal Quantity { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 生产退料
/// </summary>
public partial class PrMaterialReturn
{
    public long Id { get; set; }

    /// <summary>
    /// 生产订单ID
    /// </summary>
    public long PrOrderId { get; set; }

    /// <summary>
    /// 退料物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 退料数量
    /// </summary>
    public decimal ReturnQty { get; set; }

    /// <summary>
    /// 退料时间
    /// </summary>
    public DateTime? ReturnTime { get; set; }
}

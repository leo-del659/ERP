using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 盘点明细
/// </summary>
public partial class InvCheckDetail
{
    public long Id { get; set; }

    /// <summary>
    /// 盘点单ID
    /// </summary>
    public long CheckId { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 账面数量
    /// </summary>
    public decimal? BookQty { get; set; }

    /// <summary>
    /// 实盘数量
    /// </summary>
    public decimal? ActualQty { get; set; }

    /// <summary>
    /// 差异数量
    /// </summary>
    public decimal? DiffQty { get; set; }
}

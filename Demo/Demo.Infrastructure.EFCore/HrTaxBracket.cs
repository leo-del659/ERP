using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 个税累进税率表(月度)
/// </summary>
public partial class HrTaxBracket
{
    public long Id { get; set; }

    /// <summary>级次</summary>
    public int Level { get; set; }

    /// <summary>应纳税所得额下限</summary>
    public decimal? ThresholdLow { get; set; }

    /// <summary>应纳税所得额上限(0=无上限)</summary>
    public decimal? ThresholdHigh { get; set; }

    /// <summary>税率</summary>
    public decimal? Rate { get; set; }

    /// <summary>速算扣除数</summary>
    public decimal? QuickDeduction { get; set; }

    public DateTime? CreateTime { get; set; }

}
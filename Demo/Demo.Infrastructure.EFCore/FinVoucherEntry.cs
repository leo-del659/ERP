using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 凭证分录
/// </summary>
public partial class FinVoucherEntry
{
    public long Id { get; set; }

    /// <summary>
    /// 凭证ID
    /// </summary>
    public long VoucherId { get; set; }

    /// <summary>
    /// 分录行号
    /// </summary>
    public int EntryNo { get; set; }

    /// <summary>
    /// 会计科目编码
    /// </summary>
    public string AccountCode { get; set; } = null!;

    /// <summary>
    /// 方向：DR借/CR贷
    /// </summary>
    public string Direction { get; set; } = null!;

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 摘要
    /// </summary>
    public string? Memo { get; set; }
}

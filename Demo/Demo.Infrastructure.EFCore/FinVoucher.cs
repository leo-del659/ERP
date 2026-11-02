using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 财务凭证主表
/// </summary>
public partial class FinVoucher
{
    public long Id { get; set; }

    /// <summary>
    /// 凭证号
    /// </summary>
    public string VoucherNo { get; set; } = null!;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateOnly VoucherDate { get; set; }

    /// <summary>
    /// 来源模块：PO/SO/INV/PR
    /// </summary>
    public string? SourceModule { get; set; }

    /// <summary>
    /// 来源单据ID
    /// </summary>
    public long? SourceId { get; set; }

    /// <summary>
    /// 借方合计
    /// </summary>
    public decimal? TotalDebit { get; set; }

    /// <summary>
    /// 贷方合计
    /// </summary>
    public decimal? TotalCredit { get; set; }

    /// <summary>
    /// 0未审核/1已审核
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    public DateTime? CreateTime { get; set; }
}

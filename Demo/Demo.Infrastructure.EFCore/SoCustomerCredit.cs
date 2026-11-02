using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 客户信用
/// </summary>
public partial class SoCustomerCredit
{
    public long Id { get; set; }

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 信用额度
    /// </summary>
    public decimal? CreditLimit { get; set; }

    /// <summary>
    /// 已用额度
    /// </summary>
    public decimal? UsedAmount { get; set; }

    /// <summary>
    /// 1正常/0冻结
    /// </summary>
    public sbyte? Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    public DateTime? UpdateTime { get; set; }
}

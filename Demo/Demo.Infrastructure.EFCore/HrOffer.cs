using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 录用offer
/// </summary>
public partial class HrOffer
{
    public long Id { get; set; }

    /// <summary>候选人ID</summary>
    public long CandidateId { get; set; }

    /// <summary>录用单号</summary>
    public string OfferNo { get; set; } = null!;

    /// <summary>offer 薪资</summary>
    public decimal? Salary { get; set; }

    /// <summary>期望入职日期</summary>
    public DateOnly? EffDate { get; set; }

    /// <summary>0待确认/1接受/2拒绝</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
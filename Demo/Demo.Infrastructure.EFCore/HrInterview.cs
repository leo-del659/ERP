using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 面试记录
/// </summary>
public partial class HrInterview
{
    public long Id { get; set; }

    /// <summary>候选人ID</summary>
    public long CandidateId { get; set; }

    /// <summary>面试轮次</summary>
    public int? Round { get; set; }

    public DateOnly? InterviewDate { get; set; }

    /// <summary>面试官</summary>
    public string? Interviewers { get; set; }

    /// <summary>综合评分</summary>
    public decimal? Score { get; set; }

    /// <summary>1通过/2未过</summary>
    public sbyte? Result { get; set; }

    public DateTime? CreateTime { get; set; }

}
using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 候选人/人才库
/// </summary>
public partial class HrCandidate
{
    public long Id { get; set; }

    /// <summary>候选人姓名</summary>
    public string Name { get; set; } = null!;

    /// <summary>0女/1男</summary>
    public sbyte? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Education { get; set; }

    /// <summary>1内推/2招聘网/3校招/4猎头</summary>
    public sbyte? Source { get; set; }

    public string? ResumeUrl { get; set; }

    /// <summary>应聘岗位</summary>
    public long? PosId { get; set; }

    /// <summary>0初筛/1面试中/2已offer/3已入职/4淘汰/5人才库</summary>
    public sbyte? Status { get; set; }

    /// <summary>评分(0~100)</summary>
    public int? Rating { get; set; }

    /// <summary>0否/1是（进入人才库）</summary>
    public sbyte? TalentPool { get; set; }

    public DateTime? CreateTime { get; set; }

}
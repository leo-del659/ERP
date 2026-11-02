using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 培训需求
/// </summary>
public partial class HrTrainNeed
{
    public long Id { get; set; }

    /// <summary>培训需求单号</summary>
    public string NeedNo { get; set; } = null!;

    /// <summary>提出部门</summary>
    public long? DeptId { get; set; }

    /// <summary>需求描述</summary>
    public string? NeedDesc { get; set; }

    /// <summary>1高/2中/3低</summary>
    public sbyte? Priority { get; set; }

    /// <summary>0待审批/1采纳/2驳回</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
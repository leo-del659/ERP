using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 岗位体系
/// </summary>
public partial class HrPosition
{
    public long Id { get; set; }

    /// <summary>岗位编码</summary>
    public string PosNo { get; set; } = null!;

    /// <summary>岗位名称</summary>
    public string PosName { get; set; } = null!;

    /// <summary>归属部门</summary>
    public long? DeptId { get; set; }

    /// <summary>上级岗位ID，0=顶级</summary>
    public long? ParentId { get; set; }

    /// <summary>职级（1~20）</summary>
    public int? PosLevel { get; set; }

    /// <summary>职族/序列：管理/专业/操作</summary>
    public string? JobFamily { get; set; }

    /// <summary>岗位职责</summary>
    public string? Responsibility { get; set; }

    /// <summary>0停用/1启用</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
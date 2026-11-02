using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 部门角色关联（RBAC）
/// </summary>
public partial class SysDeptRole
{
    /// <summary>
    /// 主键（GUID，NewSequentialGuid）
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 部门ID
    /// </summary>
    public long DepartmentId { get; set; }

    /// <summary>
    /// 角色ID（sys_role.id）
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 是否禁用：1禁用/0正常
    /// </summary>
    public sbyte IsBan { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
}

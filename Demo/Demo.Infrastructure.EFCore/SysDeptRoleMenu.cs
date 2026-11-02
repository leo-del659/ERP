using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 部门角色菜单关联（RBAC）
/// </summary>
public partial class SysDeptRoleMenu
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 部门角色ID（sys_dept_role.id）
    /// </summary>
    public string DeptRoleId { get; set; } = null!;

    /// <summary>
    /// 菜单ID（sys_menu.id）
    /// </summary>
    public string MenuId { get; set; } = null!;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
}

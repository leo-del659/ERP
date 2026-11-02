using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 系统角色
/// </summary>
public partial class SysRole
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    public string RoleCode { get; set; } = null!;

    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// 状态：1启用/0停用
    /// </summary>
    public sbyte? Status { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 系统用户
/// </summary>
public partial class SysUser
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 登录名
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// 密码（加密存储）
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 状态：1启用/0停用
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 系统菜单（RBAC）
/// </summary>
public partial class SysMenu
{
    /// <summary>
    /// 主键（GUID，NewSequentialGuid）
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 路由/权限路径（如 /sys/menu，对应 Permission_Attribute 的 key）
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 类型：1目录/2菜单/3按钮
    /// </summary>
    public sbyte Type { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否禁用：1禁用/0正常
    /// </summary>
    public sbyte IsBan { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
}

using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 操作日志
/// </summary>
public partial class SysOperLog
{
    public long Id { get; set; }

    /// <summary>
    /// 操作人ID
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 模块
    /// </summary>
    public string? Module { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// 操作详情
    /// </summary>
    public string? Detail { get; set; }

    public DateTime? OperTime { get; set; }
}

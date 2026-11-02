using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 工艺路线
/// </summary>
public partial class MesRouting
{
    /// <summary>主键</summary>
    public long Id { get; set; }

    /// <summary>工艺路线编码</summary>
    public string RoutingCode { get; set; } = null!;

    /// <summary>产品ID</summary>
    public long ProductId { get; set; }

    /// <summary>版本</summary>
    public string? Version { get; set; }

    public sbyte Status { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

}
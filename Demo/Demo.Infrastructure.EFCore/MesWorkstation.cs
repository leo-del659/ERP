using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 工位/产线
/// </summary>
public partial class MesWorkstation
{
    /// <summary>主键</summary>
    public long Id { get; set; }

    /// <summary>工位编码</summary>
    public string Code { get; set; } = null!;

    /// <summary>工位名称</summary>
    public string Name { get; set; } = null!;

    /// <summary>产线</summary>
    public string? Line { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }

}
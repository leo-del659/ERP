using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 会计科目
/// </summary>
public partial class MdAccount
{
    /// <summary>
    /// 科目ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountCode { get; set; } = null!;

    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountName { get; set; } = null!;

    /// <summary>
    /// 科目类型：资产/负债/权益/成本/损益
    /// </summary>
    public string AccountType { get; set; } = null!;

    /// <summary>
    /// 上级科目ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 状态：1启用/0停用
    /// </summary>
    public sbyte? Status { get; set; }
}

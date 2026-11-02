using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 客户档案
/// </summary>
public partial class MdCustomer
{
    /// <summary>
    /// 客户ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 客户编码
    /// </summary>
    public string CustomerCode { get; set; } = null!;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = null!;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 状态：1启用/0停用
    /// </summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

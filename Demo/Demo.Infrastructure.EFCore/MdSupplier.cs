using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 供应商档案
/// </summary>
public partial class MdSupplier
{
    /// <summary>
    /// 供应商ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; } = null!;

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; } = null!;

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

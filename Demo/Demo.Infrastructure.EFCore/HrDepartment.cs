using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 部门（组织层级）
/// </summary>
public partial class HrDepartment
{
    public long Id { get; set; }

    /// <summary>部门编码</summary>
    public string DeptNo { get; set; } = null!;

    /// <summary>部门名称</summary>
    public string DeptName { get; set; } = null!;

    /// <summary>上级部门ID，0=顶级</summary>
    public long? ParentId { get; set; }

    /// <summary>层级深度</summary>
    public int? DeptLevel { get; set; }

    /// <summary>负责人(员工ID)</summary>
    public long? ManagerId { get; set; }

    /// <summary>负责人姓名（手动填写）</summary>
    public string? ManagerName { get; set; }

    /// <summary>联系电话</summary>
    public string? Phone { get; set; }

    /// <summary>办公地点</summary>
    public string? Address { get; set; }

    /// <summary>排序</summary>
    public int? OrderNo { get; set; }

    /// <summary>0停用/1启用</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
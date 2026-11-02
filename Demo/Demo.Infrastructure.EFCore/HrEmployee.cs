using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 员工主档
/// </summary>
public partial class HrEmployee
{
    public long Id { get; set; }

    /// <summary>工号</summary>
    public string EmpNo { get; set; } = null!;

    /// <summary>姓名</summary>
    public string Name { get; set; } = null!;

    /// <summary>0女/1男</summary>
    public sbyte? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    /// <summary>身份证</summary>
    public string? IdCard { get; set; }

    /// <summary>民族</summary>
    public string? Nation { get; set; }

    /// <summary>0未婚/1已婚</summary>
    public sbyte? Marital { get; set; }

    /// <summary>学历</summary>
    public string? Education { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    /// <summary>所属部门</summary>
    public long? DeptId { get; set; }

    /// <summary>所属岗位</summary>
    public long? PosId { get; set; }

    /// <summary>0试用/1正式/2休假/3离职/4借调</summary>
    public sbyte? EmpStatus { get; set; }

    /// <summary>入职日期</summary>
    public DateOnly? HireDate { get; set; }

    /// <summary>转正日期</summary>
    public DateOnly? RegularDate { get; set; }

    /// <summary>离职日期</summary>
    public DateOnly? LeaveDate { get; set; }

    /// <summary>合同份数</summary>
    public int? ContractCount { get; set; }

    public DateTime? CreateTime { get; set; }

}
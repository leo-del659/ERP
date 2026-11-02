using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 员工薪资档案(定薪)
/// </summary>
public partial class HrEmployeeSalary
{
    public long Id { get; set; }

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    /// <summary>核定基本工资</summary>
    public decimal? BaseSalary { get; set; }

    /// <summary>生效日期</summary>
    public DateOnly? EffDate { get; set; }

    /// <summary>0历史/1生效</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 薪资项目（自定义）
/// </summary>
public partial class HrSalaryItem
{
    public long Id { get; set; }

    /// <summary>薪资项目编码</summary>
    public string ItemNo { get; set; } = null!;

    /// <summary>项目名称</summary>
    public string ItemName { get; set; } = null!;

    /// <summary>1应发/2扣减</summary>
    public sbyte? ItemType { get; set; }

    /// <summary>1固定/2公式/3考勤/4绩效</summary>
    public sbyte? CalcType { get; set; }

    /// <summary>0不计税/1计税</summary>
    public sbyte? TaxFlag { get; set; }

    /// <summary>显示顺序</summary>
    public int? OrderNo { get; set; }

    public DateTime? CreateTime { get; set; }

}
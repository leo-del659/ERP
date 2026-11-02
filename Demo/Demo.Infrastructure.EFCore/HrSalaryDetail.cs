using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 薪资明细
/// </summary>
public partial class HrSalaryDetail
{
    public long Id { get; set; }

    /// <summary>工资单ID</summary>
    public long SalaryId { get; set; }

    /// <summary>薪资项目ID</summary>
    public long? ItemId { get; set; }

    public string? ItemName { get; set; }

    public decimal? Amount { get; set; }

    /// <summary>1收入/-1扣减</summary>
    public sbyte? Direction { get; set; }

    public DateTime? CreateTime { get; set; }

}
using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 薪资核算主表
/// </summary>
public partial class HrSalary
{
    public long Id { get; set; }

    /// <summary>工资单号</summary>
    public string SalaryNo { get; set; } = null!;

    /// <summary>薪资期间(YYYY-MM)</summary>
    public string Period { get; set; } = null!;

    /// <summary>员工ID</summary>
    public long EmpId { get; set; }

    public decimal? BaseSalary { get; set; }

    /// <summary>出勤天数</summary>
    public decimal? AttendDays { get; set; }

    /// <summary>加班费(来自 hr_attendance/overtime)</summary>
    public decimal? OtPay { get; set; }

    /// <summary>请假扣款(来自 hr_leave)</summary>
    public decimal? LeaveDeduct { get; set; }

    /// <summary>绩效奖金(来自 hr_perf_result)</summary>
    public decimal? PerfBonus { get; set; }

    /// <summary>应发合计</summary>
    public decimal? GrossPay { get; set; }

    /// <summary>个人社保</summary>
    public decimal? SocialPersonal { get; set; }

    /// <summary>个人公积金</summary>
    public decimal? FundPersonal { get; set; }

    /// <summary>个税(按 hr_tax_bracket)</summary>
    public decimal? Tax { get; set; }

    /// <summary>实发合计</summary>
    public decimal? NetPay { get; set; }

    /// <summary>关联 fin_voucher.id（传递财务总账）</summary>
    public long? VoucherId { get; set; }

    /// <summary>0草稿/1已核算/2已发/3已生成凭证</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }

}
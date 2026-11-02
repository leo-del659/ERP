using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 社保/公积金缴费规则
/// </summary>
public partial class HrSocialRule
{
    public long Id { get; set; }

    /// <summary>参保城市</summary>
    public string City { get; set; } = null!;

    /// <summary>缴费基数下限</summary>
    public decimal? BaseMin { get; set; }

    /// <summary>缴费基数上限</summary>
    public decimal? BaseMax { get; set; }

    /// <summary>养老-个人比例</summary>
    public decimal? PensionEmpRate { get; set; }

    /// <summary>养老-企业比例</summary>
    public decimal? PensionComRate { get; set; }

    /// <summary>医疗-个人比例</summary>
    public decimal? MedicalEmpRate { get; set; }

    /// <summary>医疗-企业比例</summary>
    public decimal? MedicalComRate { get; set; }

    /// <summary>失业-个人比例</summary>
    public decimal? UnemploymentEmpRate { get; set; }

    /// <summary>失业-企业比例</summary>
    public decimal? UnemploymentComRate { get; set; }

    /// <summary>工伤-企业比例</summary>
    public decimal? InjuryComRate { get; set; }

    /// <summary>生育-企业比例</summary>
    public decimal? BirthComRate { get; set; }

    /// <summary>公积金-个人比例</summary>
    public decimal? FundEmpRate { get; set; }

    /// <summary>公积金-企业比例</summary>
    public decimal? FundComRate { get; set; }

    public DateTime? CreateTime { get; set; }

}
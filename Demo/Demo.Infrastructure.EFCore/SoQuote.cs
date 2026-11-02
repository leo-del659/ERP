namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 销售报价单：销售模块价格策略与交期的载体。
/// </summary>
public partial class SoQuote
{
    public long Id { get; set; }

    /// <summary>报价单号</summary>
    public string QuoteNo { get; set; } = null!;

    /// <summary>客户ID</summary>
    public long CustomerId { get; set; }

    /// <summary>报价日期</summary>
    public DateOnly QuoteDate { get; set; }

    /// <summary>报价有效期</summary>
    public DateOnly? ValidDate { get; set; }

    /// <summary>价格策略：STANDARD标准/PREFER最低价/VIP折扣/CUSTOM协议价</summary>
    public string? PriceStrategy { get; set; } = "STANDARD";

    /// <summary>折扣率（1=不打折）</summary>
    public decimal? DiscountRate { get; set; } = 1m;

    /// <summary>报价总额</summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>状态：0草稿/1已生效/2已转订单/3已失效</summary>
    public sbyte Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    /// <summary>转生成的销售订单ID</summary>
    public long? OrderId { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    public DateTime CreateTime { get; set; }
}

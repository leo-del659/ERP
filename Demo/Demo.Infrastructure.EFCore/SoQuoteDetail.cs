namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 销售报价单明细
/// </summary>
public partial class SoQuoteDetail
{
    public long Id { get; set; }

    /// <summary>报价单ID</summary>
    public long QuoteId { get; set; }

    /// <summary>物料ID</summary>
    public long MaterialId { get; set; }

    /// <summary>数量</summary>
    public decimal Quantity { get; set; }

    /// <summary>单价</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>行折扣率</summary>
    public decimal? DiscountRate { get; set; } = 1m;

    /// <summary>行金额</summary>
    public decimal? Amount { get; set; }
}

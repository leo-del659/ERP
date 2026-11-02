namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 请购单明细
/// </summary>
public partial class PoRequisitionDetail
{
    public long Id { get; set; }

    /// <summary>请购单ID</summary>
    public long ReqId { get; set; }

    /// <summary>物料ID</summary>
    public long MaterialId { get; set; }

    /// <summary>请购数量</summary>
    public decimal ReqQty { get; set; }

    /// <summary>已转订单数量</summary>
    public decimal? ConvertedQty { get; set; }

    /// <summary>建议供应商ID（按报价最优价自动推荐）</summary>
    public long? SuggestSupplierId { get; set; }

    /// <summary>建议单价</summary>
    public decimal? SuggestPrice { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }
}

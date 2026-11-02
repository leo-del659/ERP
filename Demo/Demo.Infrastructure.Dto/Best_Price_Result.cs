namespace Demo.Infrastructure.Dto;

/// <summary>
/// 物料最优报价结果（供应商报价中在有效期内取最低价）。
/// </summary>
public class Best_Price_Result
{
    /// <summary>供应商ID</summary>
    public long SupplierId { get; set; }

    /// <summary>供应商名称</summary>
    public string? SupplierName { get; set; }

    /// <summary>报价单价</summary>
    public decimal Price { get; set; }

    /// <summary>有效期起</summary>
    public DateOnly? ValidFrom { get; set; }

    /// <summary>有效期止</summary>
    public DateOnly? ValidTo { get; set; }
}

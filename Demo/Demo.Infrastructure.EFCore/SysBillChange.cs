namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 单据变更记录：销售/采购订单变更留痕，便于追溯变更前后值与原因。
/// </summary>
public partial class SysBillChange
{
    public long Id { get; set; }

    /// <summary>单据类型：SO/PO</summary>
    public string BillType { get; set; } = null!;

    /// <summary>单据ID</summary>
    public long BillId { get; set; }

    /// <summary>单据号</summary>
    public string? BillNo { get; set; }

    /// <summary>变更类型：QTY数量/PRICE价格/DATE交期/ADD新增行/DEL删除行</summary>
    public string ChangeType { get; set; } = null!;

    /// <summary>变更前</summary>
    public string? BeforeValue { get; set; }

    /// <summary>变更后</summary>
    public string? AfterValue { get; set; }

    /// <summary>变更原因</summary>
    public string? Reason { get; set; }

    /// <summary>操作人</summary>
    public string? OperUser { get; set; }

    public DateTime OperTime { get; set; }
}

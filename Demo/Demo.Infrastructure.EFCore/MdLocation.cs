namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 库位档案：仓库内的具体存储位（区域/货架），用于库存精细化管理。
/// </summary>
public partial class MdLocation
{
    public long Id { get; set; }

    /// <summary>所属仓库ID</summary>
    public long WarehouseId { get; set; }

    /// <summary>库位编码</summary>
    public string LocationCode { get; set; } = null!;

    /// <summary>库位名称</summary>
    public string? LocationName { get; set; }

    /// <summary>区域</summary>
    public string? Area { get; set; }

    /// <summary>货架</summary>
    public string? Shelf { get; set; }

    /// <summary>状态：1启用/0停用</summary>
    public sbyte? Status { get; set; }

    public DateTime? CreateTime { get; set; }
}

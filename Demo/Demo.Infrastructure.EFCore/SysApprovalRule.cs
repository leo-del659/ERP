namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 审批规则：按业务类型 + 金额区间，决定审批级数及各节点的审批角色。
/// 提交审批单时由 Approval_Service 自动匹配，无需前端指定级数/角色。
/// 示例：采购订单金额 ≥ 50万 → 3 级审批，节点角色 "deptManager,financeDirector,generalManager"。
/// </summary>
public partial class SysApprovalRule
{
    public long Id { get; set; }

    /// <summary>业务类型：PO/SO/PR/QUOTE/VOUCHER/PAYMENT/INV/MD/OTHER</summary>
    public string BizType { get; set; } = null!;

    /// <summary>金额下限（含）</summary>
    public decimal MinAmount { get; set; }

    /// <summary>金额上限（不含），null 表示无上限</summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>审批节点级数</summary>
    public int Level { get; set; } = 1;

    /// <summary>
    /// 各节点审批角色序列，逗号分隔；第 n 个值对应第 n 个审批节点。
    /// 如 "deptManager,financeDirector,generalManager"。
    /// </summary>
    public string NodeRoles { get; set; } = "admin";

    /// <summary>是否启用：1启用/0停用</summary>
    public sbyte? Enabled { get; set; } = 1;

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    public DateTime CreateTime { get; set; }
}

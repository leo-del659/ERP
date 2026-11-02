namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 审批记录：记录审批单每一次流转动作，形成完整审批留痕。
/// </summary>
public partial class SysApprovalLog
{
    public long Id { get; set; }

    /// <summary>审批单ID</summary>
    public long ApprovalId { get; set; }

    /// <summary>节点序号，0=提交</summary>
    public int NodeNo { get; set; }

    /// <summary>动作：1提交/2通过/3驳回/4撤回</summary>
    public sbyte Action { get; set; }

    /// <summary>审批人ID</summary>
    public long? ApproverId { get; set; }

    /// <summary>审批人姓名</summary>
    public string? ApproverName { get; set; }

    /// <summary>审批意见</summary>
    public string? Opinion { get; set; }

    public DateTime OperTime { get; set; }
}

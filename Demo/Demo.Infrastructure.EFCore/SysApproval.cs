namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 审批单主表。支撑销售/采购/生产/凭证等单据的通用审批流转。
/// </summary>
public partial class SysApproval
{
    public long Id { get; set; }

    /// <summary>审批单号</summary>
    public string ApprovalNo { get; set; } = null!;

    /// <summary>单据类型：SO/PO/PR/VOUCHER/PO_REQ/OTHER</summary>
    public string BizType { get; set; } = null!;

    /// <summary>业务单据ID</summary>
    public long? BizId { get; set; }

    /// <summary>业务单据号（冗余，便于列表直接展示）</summary>
    public string? BillNo { get; set; }

    /// <summary>审批标题</summary>
    public string Title { get; set; } = null!;

    /// <summary>金额</summary>
    public decimal? Amount { get; set; }

    /// <summary>申请人ID</summary>
    public long? ApplicantId { get; set; }

    /// <summary>申请人姓名</summary>
    public string? ApplicantName { get; set; }

    /// <summary>状态：0草稿/1审批中/2已通过/3已驳回</summary>
    public sbyte Status { get; set; }

    /// <summary>当前审批节点序号</summary>
    public int CurrentNode { get; set; }

    /// <summary>审批节点总数</summary>
    public int TotalNodes { get; set; } = 1;

    /// <summary>各节点审批角色序列（逗号分隔，第 n 个值对应第 n 个节点），如 "deptManager,financeDirector,admin"</summary>
    public string? NodeRoles { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    /// <summary>提交时间</summary>
    public DateTime? SubmitTime { get; set; }

    /// <summary>审批完成时间</summary>
    public DateTime? FinishTime { get; set; }

    public DateTime CreateTime { get; set; }
}

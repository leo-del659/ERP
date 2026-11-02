namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 请购单：采购模块的请购 → 下单前序单据。
/// </summary>
public partial class PoRequisition
{
    public long Id { get; set; }

    /// <summary>请购单号</summary>
    public string ReqNo { get; set; } = null!;

    /// <summary>请购日期</summary>
    public DateOnly ReqDate { get; set; }

    /// <summary>申请部门</summary>
    public string? DeptName { get; set; }

    /// <summary>申请人ID</summary>
    public long? ApplicantId { get; set; }

    /// <summary>申请人姓名</summary>
    public string? ApplicantName { get; set; }

    /// <summary>状态：0草稿/1已转采购订单/2已驳回</summary>
    public sbyte Status { get; set; }

    /// <summary>审批状态：0待审/1通过/2驳回</summary>
    public sbyte? AuditStatus { get; set; }

    /// <summary>转生成的采购订单ID</summary>
    public long? PoOrderId { get; set; }

    /// <summary>备注</summary>
    public string? Remark { get; set; }

    public DateTime CreateTime { get; set; }
}

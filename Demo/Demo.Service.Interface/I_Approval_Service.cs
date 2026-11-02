using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.EFCore;

namespace Demo.Service.Interface;

/// <summary>
/// 通用审批流服务：单据提交 → 逐级审批 → 通过/驳回，
/// 审批通过后自动回写业务单据状态（采购订单转已审核、销售订单转已确认）。
/// </summary>
[Provider_Attribute]
public interface I_Approval_Service
{
    /// <summary>提交审批单（草稿 → 审批中，按规则自动计算级数/节点角色）</summary>
    Task<object> SubmitAsync(Approval_Submit_Request req);

    /// <summary>审批处理：通过 / 驳回（按当前节点角色校验权限）</summary>
    Task<object> HandleAsync(long id, Approval_Handle_Request req, string? approverRole, string? approverName);

    /// <summary>撤回审批单（回到草稿）</summary>
    Task<object> CancelAsync(long id, string? reason);

    /// <summary>审批单分页列表</summary>
    Task<object> PageAsync(Page_Request req, sbyte? status);

    /// <summary>审批单详情（含审批留痕）</summary>
    Task<object> DetailAsync(long id);

    /// <summary>待我审批数量</summary>
    Task<object> PendingCountAsync();

    /// <summary>某业务单据的审批单</summary>
    Task<object> GetByBizAsync(string bizType, long bizId);

    /// <summary>审批规则列表（按业务类型过滤）</summary>
    Task<object> GetRulesAsync(string? bizType);

    /// <summary>保存（新增/更新）审批规则</summary>
    Task<object> SaveRuleAsync(Approval_Rule_Save_Request req);

    /// <summary>删除审批规则</summary>
    Task<object> DeleteRuleAsync(long id);
}

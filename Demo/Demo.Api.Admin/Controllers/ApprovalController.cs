using System.Security.Claims;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 审批流：单据提交 → 逐级审批 → 通过后自动回写业务单据状态。
/// </summary>
[ApiController]
[Authorize]
[Permission_("/sys/approval")]
[Route("api/[controller]")]
public class ApprovalController : ControllerBase
{
    private readonly I_Approval_Service _svc;
    public ApprovalController(I_Approval_Service svc) => _svc = svc;

    /// <summary>提交审批单</summary>
    [HttpPost("submit")]
    [ActiconLog_(group = "审批流", action = "提交审批")]
    public async Task<Api_Response> Submit([FromBody] Approval_Submit_Request req)
        => Api_Response.Ok(await _svc.SubmitAsync(req));

    /// <summary>审批处理（通过 / 驳回）</summary>
    [HttpPost("{id:long}/handle")]
    [ActiconLog_(group = "审批流", action = "审批处理")]
    public async Task<Api_Response> Handle(long id, [FromBody] Approval_Handle_Request req)
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var name = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name ?? "匿名";
        return Api_Response.Ok(await _svc.HandleAsync(id, req, role, name));
    }

    /// <summary>撤回审批单</summary>
    [HttpPost("{id:long}/cancel")]
    [ActiconLog_(group = "审批流", action = "撤回")]
    public async Task<Api_Response> Cancel(long id, [FromBody] Approval_Handle_Request? req)
        => Api_Response.Ok(await _svc.CancelAsync(id, req?.opinion));

    /// <summary>审批单分页列表</summary>
    [HttpGet]
    public async Task<Api_Response> Page([FromQuery] Page_Request req, [FromQuery] sbyte? status)
        => Api_Response.Ok(await _svc.PageAsync(req, status));

    /// <summary>审批单详情（含审批留痕）</summary>
    [HttpGet("{id:long}")]
    public async Task<Api_Response> Detail(long id)
        => Api_Response.Ok(await _svc.DetailAsync(id));

    /// <summary>待我审批数量</summary>
    [HttpGet("pending-count")]
    public async Task<Api_Response> PendingCount()
        => Api_Response.Ok(await _svc.PendingCountAsync());

    /// <summary>查询某业务单据的审批单</summary>
    [HttpGet("biz/{bizType}/{bizId:long}")]
    public async Task<Api_Response> ByBiz(string bizType, long bizId)
        => Api_Response.Ok(await _svc.GetByBizAsync(bizType, bizId));

    /// <summary>审批规则列表（按业务类型过滤）</summary>
    [HttpGet("rule")]
    public async Task<Api_Response> Rules([FromQuery] string? bizType)
        => Api_Response.Ok(await _svc.GetRulesAsync(bizType));

    /// <summary>保存（新增/更新）审批规则</summary>
    [HttpPost("rule")]
    [ActiconLog_(group = "审批流", action = "保存审批规则")]
    public async Task<Api_Response> SaveRule([FromBody] Approval_Rule_Save_Request req)
        => Api_Response.Ok(await _svc.SaveRuleAsync(req));

    /// <summary>删除审批规则</summary>
    [HttpDelete("rule/{id:long}")]
    [ActiconLog_(group = "审批流", action = "删除审批规则")]
    public async Task<Api_Response> DeleteRule(long id)
        => Api_Response.Ok(await _svc.DeleteRuleAsync(id));
}

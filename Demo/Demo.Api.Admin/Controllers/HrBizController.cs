using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块业务级联接口（细粒度权限，独立于通用 CRUD）：
/// 员工异动→合同状态、请假/加班审批→考勤、薪资核算→财务凭证。
/// </summary>
[ApiController]
[Authorize]
[Route("api/hr/biz")]
public class HrBizController : ControllerBase
{
    private readonly I_Hr_Service _hr;

    public HrBizController(I_Hr_Service hr)
    {
        _hr = hr;
    }

    /// <summary>处理员工异动（联动员工与合同状态）</summary>
    [HttpPost("employee-change/{id}/process")]
    [Permission_("/hr/employee-change")]
    [ActiconLog_Attribute(group = "HR", action = "处理员工异动")]
    public async Task<Api_Response> ProcessChange(long id) =>
        Api_Response.Ok(await _hr.ProcessEmployeeChangeAsync(id, User.Identity?.Name));

    /// <summary>薪资核算（考勤/绩效/定薪自动算薪并生成财务凭证）</summary>
    [HttpPost("salary/calculate")]
    [Permission_("/hr/salary")]
    [ActiconLog_Attribute(group = "HR", action = "薪资核算")]
    public async Task<Api_Response> Calculate([FromBody] HrCalc_Request req) =>
        Api_Response.Ok(await _hr.CalculateSalaryAsync(req.year, req.month, req.employeeIds));

    /// <summary>请假审批通过（联动写入考勤缺勤）</summary>
    [HttpPost("leave/{id}/approve")]
    [Permission_("/hr/leave")]
    [ActiconLog_Attribute(group = "HR", action = "请假审批")]
    public async Task<Api_Response> ApproveLeave(long id) =>
        Api_Response.Ok(await _hr.ApproveLeaveAsync(id, User.Identity?.Name));

    /// <summary>加班审批通过（联动写入考勤加班）</summary>
    [HttpPost("overtime/{id}/approve")]
    [Permission_("/hr/overtime")]
    [ActiconLog_Attribute(group = "HR", action = "加班审批")]
    public async Task<Api_Response> ApproveOvertime(long id) =>
        Api_Response.Ok(await _hr.ApproveOvertimeAsync(id, User.Identity?.Name));
}

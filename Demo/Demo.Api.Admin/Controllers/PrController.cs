using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Authorize]
[Permission_("/pr")]
[Route("api/[controller]")]
public class PrController : ControllerBase
{
    private readonly I_Production_Service _svc;
    public PrController(I_Production_Service svc) => _svc = svc;

    [HttpGet]
    public async Task<Api_Response> Page([FromQuery] Page_Request req) => Api_Response.Ok(await _svc.PageAsync(req));

    [HttpPost]
    [ActiconLog_(group = "生产", action = "创建工单")]
    public async Task<Api_Response> Create([FromBody] Pr_Order_Create_Request req) => Api_Response.Ok(await _svc.CreateOrderAsync(req));

    [HttpPost("{id}/release")]
    [ActiconLog_(group = "生产", action = "下达")]
    public async Task<Api_Response> Release(long id) => Api_Response.Ok(await _svc.ReleaseAsync(id));

    [HttpPost("issue")]
    [ActiconLog_(group = "生产", action = "领料")]
    public async Task<Api_Response> Issue([FromBody] Issue_Request req) => Api_Response.Ok(await _svc.IssueAsync(req));

    [HttpPost("report")]
    [ActiconLog_(group = "生产", action = "报工")]
    public async Task<Api_Response> Report([FromBody] Report_Request req) => Api_Response.Ok(await _svc.ReportAsync(req));

    [HttpPost("{id}/complete")]
    [ActiconLog_(group = "生产", action = "完工入库")]
    public async Task<Api_Response> Complete(long id, [FromQuery] long warehouseId) => Api_Response.Ok(await _svc.CompleteAsync(id, warehouseId));

    [HttpPost("mrp")]
    [ActiconLog_(group = "生产", action = "MRP运算")]
    public async Task<Api_Response> Mrp() => Api_Response.Ok(await _svc.RunMrpAsync());

    // ==================== 主生产计划 MPS ====================

    /// <summary>新建主生产计划</summary>
    [HttpPost("mps")]
    [ActiconLog_(group = "生产", action = "新建主计划")]
    public async Task<Api_Response> CreateMps([FromBody] Mps_Create_Request req)
        => Api_Response.Ok(await _svc.CreateMpsAsync(req));

    /// <summary>主生产计划列表</summary>
    [HttpGet("mps")]
    public async Task<Api_Response> MpsPage([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.MpsPageAsync(req));

    /// <summary>确认主生产计划</summary>
    [HttpPost("mps/{id:long}/confirm")]
    [ActiconLog_(group = "生产", action = "确认主计划")]
    public async Task<Api_Response> ConfirmMps(long id)
        => Api_Response.Ok(await _svc.ConfirmMpsAsync(id));

    /// <summary>主生产计划转生产订单</summary>
    [HttpPost("mps/to-order")]
    [ActiconLog_(group = "生产", action = "主计划转工单")]
    public async Task<Api_Response> MpsToOrder([FromBody] Mps_To_Order_Request req)
        => Api_Response.Ok(await _svc.MpsToOrderAsync(req));

    // ==================== MRP 运算结果 ====================

    /// <summary>MRP 运算结果列表</summary>
    [HttpGet("mrp")]
    public async Task<Api_Response> MrpPage([FromQuery] Page_Request req, [FromQuery] string? mrpNo)
        => Api_Response.Ok(await _svc.MrpPageAsync(req, mrpNo));

    /// <summary>MRP 建议转采购订单</summary>
    [HttpPost("mrp/to-po")]
    [ActiconLog_(group = "生产", action = "MRP转采购订单")]
    public async Task<Api_Response> MrpToPo([FromBody] Mrp_To_Po_Request req)
        => Api_Response.Ok(await _svc.MrpToPoAsync(req));
}

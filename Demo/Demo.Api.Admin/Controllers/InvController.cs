using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.EFCore;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Authorize]
[Permission_("/inv")]
[Route("api/[controller]")]
public class InvController : ControllerBase
{
    private readonly I_Inventory_Service _svc;
    public InvController(I_Inventory_Service svc) => _svc = svc;

    [HttpGet("stock")]
    public async Task<Api_Response> Stock([FromQuery] Page_Request req, [FromQuery] long? warehouseId)
        => Api_Response.Ok(await _svc.StockPageAsync(req, warehouseId));

    [HttpGet("trans")]
    public async Task<Api_Response> Trans([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.TransPageAsync(req));

    [HttpGet("alerts")]
    public async Task<Api_Response> Alerts() => Api_Response.Ok(await _svc.AlertsAsync());

    [HttpPost("in")]
    [ActiconLog_(group = "库存", action = "入库")]
    public async Task<Api_Response> In([FromBody] Inv_In_Request req) => Api_Response.Ok(await _svc.ConfirmInAsync(req));

    [HttpPost("out")]
    [ActiconLog_(group = "库存", action = "出库")]
    public async Task<Api_Response> Out([FromBody] Inv_Out_Request req) => Api_Response.Ok(await _svc.ConfirmOutAsync(req));

    [HttpPost("transfer")]
    [ActiconLog_(group = "库存", action = "调拨")]
    public async Task<Api_Response> Transfer([FromBody] InvTransfer bill) => Api_Response.Ok(await _svc.TransferAsync(bill));

    // ==================== 库存盘点 ====================

    /// <summary>创建盘点单（按当前台账快照账面数量）</summary>
    [HttpPost("check")]
    [ActiconLog_(group = "库存", action = "创建盘点单")]
    public async Task<Api_Response> CreateCheck([FromBody] Check_Create_Request req)
        => Api_Response.Ok(await _svc.CreateCheckAsync(req));

    /// <summary>盘点单列表</summary>
    [HttpGet("check")]
    public async Task<Api_Response> CheckPage([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.CheckPageAsync(req));

    /// <summary>盘点单详情（含明细）</summary>
    [HttpGet("check/{id:long}")]
    public async Task<Api_Response> CheckDetail(long id)
        => Api_Response.Ok(await _svc.CheckDetailAsync(id));

    /// <summary>录入实盘数量并计算差异</summary>
    [HttpPost("check/finish")]
    [ActiconLog_(group = "库存", action = "录入盘点结果")]
    public async Task<Api_Response> FinishCheck([FromBody] Check_Finish_Request req)
        => Api_Response.Ok(await _svc.FinishCheckAsync(req));

    /// <summary>审核盘点单（按差异调整库存并生成流水）</summary>
    [HttpPost("check/{id:long}/audit")]
    [ActiconLog_(group = "库存", action = "审核盘点单")]
    public async Task<Api_Response> AuditCheck(long id)
        => Api_Response.Ok(await _svc.AuditCheckAsync(id));

    // ==================== 批次库存 ====================

    /// <summary>批次库存列表</summary>
    [HttpGet("batch")]
    public async Task<Api_Response> BatchPage([FromQuery] Page_Request req, [FromQuery] long? warehouseId)
        => Api_Response.Ok(await _svc.BatchPageAsync(req, warehouseId));

    /// <summary>登记批次</summary>
    [HttpPost("batch")]
    [ActiconLog_(group = "库存", action = "登记批次")]
    public async Task<Api_Response> SaveBatch([FromBody] Batch_Save_Request req)
        => Api_Response.Ok(await _svc.SaveBatchAsync(req));
}

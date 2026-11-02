using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Authorize]
[Permission_("/po")]
[Route("api/[controller]")]
public class PoController : ControllerBase
{
    private readonly I_Purchase_Service _svc;
    public PoController(I_Purchase_Service svc) => _svc = svc;

    [HttpGet]
    public async Task<Api_Response> Page([FromQuery] Page_Request req) => Api_Response.Ok(await _svc.PageAsync(req));

    [HttpPost]
    [ActiconLog_(group = "采购", action = "创建订单")]
    public async Task<Api_Response> Create([FromBody] Po_Create_Request req) => Api_Response.Ok(await _svc.CreateAsync(req));

    [HttpPost("{id}/approve")]
    [ActiconLog_(group = "采购", action = "审核")]
    public async Task<Api_Response> Approve(long id) => Api_Response.Ok(await _svc.ApproveAsync(id));

    [HttpPost("receive")]
    [ActiconLog_(group = "采购", action = "收货")]
    public async Task<Api_Response> Receive([FromBody] Receipt_Create_Request req) => Api_Response.Ok(await _svc.ReceiveAsync(req));

    [HttpPost("{id}/inspect")]
    [ActiconLog_(group = "采购", action = "质检入库")]
    public async Task<Api_Response> Inspect(long id, [FromQuery] bool pass = true) => Api_Response.Ok(await _svc.InspectAsync(id, pass));

    // ==================== 采购订单变更 ====================

    /// <summary>采购订单变更</summary>
    [HttpPost("change")]
    [ActiconLog_(group = "采购", action = "订单变更")]
    public async Task<Api_Response> Change([FromBody] Order_Change_Request req)
        => Api_Response.Ok(await _svc.ChangeAsync(req));

    // ==================== 请购单 ====================

    /// <summary>新建请购单</summary>
    [HttpPost("requisition")]
    [ActiconLog_(group = "采购", action = "新建请购单")]
    public async Task<Api_Response> CreateRequisition([FromBody] Requisition_Create_Request req)
        => Api_Response.Ok(await _svc.CreateRequisitionAsync(req));

    /// <summary>请购单列表</summary>
    [HttpGet("requisition")]
    public async Task<Api_Response> RequisitionPage([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.RequisitionPageAsync(req));

    /// <summary>请购转采购订单</summary>
    [HttpPost("requisition/to-po")]
    [ActiconLog_(group = "采购", action = "请购转订单")]
    public async Task<Api_Response> RequisitionToPo([FromBody] Requisition_To_Po_Request req)
        => Api_Response.Ok(await _svc.RequisitionToPoAsync(req));

    // ==================== 供应商报价与绩效 ====================

    /// <summary>登记供应商报价</summary>
    [HttpPost("quote")]
    [ActiconLog_(group = "采购", action = "登记报价")]
    public async Task<Api_Response> SaveQuote([FromBody] Supplier_Quote_Request req)
        => Api_Response.Ok(await _svc.SaveQuoteAsync(req));

    /// <summary>供应商报价列表</summary>
    [HttpGet("quote")]
    public async Task<Api_Response> QuotePage([FromQuery] Page_Request req, [FromQuery] long? materialId)
        => Api_Response.Ok(await _svc.QuotePageAsync(req, materialId));

    /// <summary>物料最优报价</summary>
    [HttpGet("quote/best")]
    public async Task<Api_Response> BestPrice([FromQuery] long materialId)
        => Api_Response.Ok(await _svc.BestPriceAsync(materialId));

    /// <summary>供应商绩效评估</summary>
    [HttpGet("performance/{supplierId:long}")]
    public async Task<Api_Response> Performance(long supplierId)
        => Api_Response.Ok(await _svc.PerformanceAsync(supplierId));
}

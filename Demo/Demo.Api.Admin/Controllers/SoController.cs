using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Authorize]
[Permission_("/so")]
[Route("api/[controller]")]
public class SoController : ControllerBase
{
    private readonly I_Sales_Service _svc;
    public SoController(I_Sales_Service svc) => _svc = svc;

    [HttpGet]
    public async Task<Api_Response> Page([FromQuery] Page_Request req) => Api_Response.Ok(await _svc.PageAsync(req));

    [HttpPost]
    [ActiconLog_(group = "销售", action = "创建订单")]
    public async Task<Api_Response> Create([FromBody] So_Create_Request req) => Api_Response.Ok(await _svc.CreateAsync(req));

    [HttpPost("{id}/confirm")]
    [ActiconLog_(group = "销售", action = "确认订单")]
    public async Task<Api_Response> Confirm(long id) => Api_Response.Ok(await _svc.ConfirmAsync(id));

    [HttpPost("deliver")]
    [ActiconLog_(group = "销售", action = "发货")]
    public async Task<Api_Response> Deliver([FromBody] Delivery_Create_Request req) => Api_Response.Ok(await _svc.DeliverAsync(req));

    [HttpPost("{id}/complete")]
    [ActiconLog_(group = "销售", action = "完成")]
    public async Task<Api_Response> Complete(long id) => Api_Response.Ok(await _svc.CompleteAsync(id));

    // ==================== 销售报价与价格策略 ====================

    /// <summary>新建报价单</summary>
    [HttpPost("quote")]
    [ActiconLog_(group = "销售", action = "新建报价")]
    public async Task<Api_Response> CreateQuote([FromBody] Quote_Create_Request req)
        => Api_Response.Ok(await _svc.CreateQuoteAsync(req));

    /// <summary>报价单列表</summary>
    [HttpGet("quote")]
    public async Task<Api_Response> QuotePage([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.QuotePageAsync(req));

    /// <summary>报价单详情</summary>
    [HttpGet("quote/{id:long}")]
    public async Task<Api_Response> QuoteDetail(long id)
        => Api_Response.Ok(await _svc.QuoteDetailAsync(id));

    /// <summary>报价单生效</summary>
    [HttpPost("quote/{id:long}/confirm")]
    [ActiconLog_(group = "销售", action = "报价生效")]
    public async Task<Api_Response> ConfirmQuote(long id)
        => Api_Response.Ok(await _svc.ConfirmQuoteAsync(id));

    /// <summary>报价转销售订单</summary>
    [HttpPost("quote/to-order")]
    [ActiconLog_(group = "销售", action = "报价转订单")]
    public async Task<Api_Response> QuoteToOrder([FromBody] Quote_To_Order_Request req)
        => Api_Response.Ok(await _svc.QuoteToOrderAsync(req));

    // ==================== 订单变更 ====================

    /// <summary>销售订单变更</summary>
    [HttpPost("change")]
    [ActiconLog_(group = "销售", action = "订单变更")]
    public async Task<Api_Response> Change([FromBody] Order_Change_Request req)
        => Api_Response.Ok(await _svc.ChangeAsync(req));

    /// <summary>订单变更留痕</summary>
    [HttpGet("change/{billType}/{billId:long}")]
    public async Task<Api_Response> ChangeLog(string billType, long billId)
        => Api_Response.Ok(await _svc.ChangeLogAsync(billType, billId));

    // ==================== 发货与物流跟踪 ====================

    /// <summary>发货单列表</summary>
    [HttpGet("delivery")]
    public async Task<Api_Response> DeliveryPage([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.DeliveryPageAsync(req));

    /// <summary>登记物流跟踪节点</summary>
    [HttpPost("track")]
    [ActiconLog_(group = "销售", action = "物流跟踪")]
    public async Task<Api_Response> AddTrack([FromBody] Track_Create_Request req)
        => Api_Response.Ok(await _svc.AddTrackAsync(req));

    /// <summary>物流跟踪记录</summary>
    [HttpGet("track/{deliveryId:long}")]
    public async Task<Api_Response> TrackList(long deliveryId)
        => Api_Response.Ok(await _svc.TrackListAsync(deliveryId));

    // ==================== 客户信用管理 ====================

    /// <summary>客户信用列表</summary>
    [HttpGet("credit")]
    public async Task<Api_Response> CreditPage([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.CreditPageAsync(req));

    /// <summary>维护客户信用额度</summary>
    [HttpPost("credit")]
    [ActiconLog_(group = "销售", action = "维护信用额度")]
    public async Task<Api_Response> SaveCredit([FromBody] SoCustomerCredit credit)
        => Api_Response.Ok(await _svc.SaveCreditAsync(credit));

    /// <summary>冻结 / 解冻客户信用</summary>
    [HttpPost("credit/{customerId:long}/freeze")]
    [ActiconLog_(group = "销售", action = "冻结信用")]
    public async Task<Api_Response> FreezeCredit(long customerId, [FromQuery] bool freeze = true)
        => Api_Response.Ok(await _svc.FreezeCreditAsync(customerId, freeze));
}

using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Authorize]
[Permission_("/fin")]
[Route("api/[controller]")]
public class FinController : ControllerBase
{
    private readonly I_Finance_Service _svc;
    public FinController(I_Finance_Service svc) => _svc = svc;

    [HttpPost("voucher")]
    [ActiconLog_(group = "财务", action = "录凭证")]
    public async Task<Api_Response> Voucher([FromBody] Voucher_Create_Request req) => Api_Response.Ok(await _svc.CreateVoucherAsync(req));

    [HttpPost("voucher/{id}/audit")]
    public async Task<Api_Response> Audit(long id) => Api_Response.Ok(await _svc.AuditVoucherAsync(id));

    [HttpPost("voucher/{id}/post")]
    public async Task<Api_Response> Post(long id) => Api_Response.Ok(await _svc.PostVoucherAsync(id));

    [HttpPost("voucher/{id}/close")]
    public async Task<Api_Response> Close(long id) => Api_Response.Ok(await _svc.CloseVoucherAsync(id));

    [HttpPost("receipt")]
    [ActiconLog_(group = "财务", action = "收款核销")]
    public async Task<Api_Response> Receipt([FromBody] Receipt_Writeoff_Request req) => Api_Response.Ok(await _svc.WriteoffReceiptAsync(req));

    [HttpPost("payment")]
    [ActiconLog_(group = "财务", action = "付款核销")]
    public async Task<Api_Response> Payment([FromBody] Payment_Writeoff_Request req) => Api_Response.Ok(await _svc.WriteoffPaymentAsync(req));

    [HttpGet("ageing")]
    public async Task<Api_Response> Ageing() => Api_Response.Ok(await _svc.AgeingAsync());

    // 总账：凭证列表 / 凭证明细
    [HttpGet("voucher")]
    public async Task<Api_Response> PageVoucher([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.PageVoucherAsync(req));

    [HttpGet("voucher/{id:long}")]
    public async Task<Api_Response> VoucherDetail(long id)
        => Api_Response.Ok(await _svc.VoucherDetailAsync(id));

    // 应收 / 应付单列表（供核销选择）
    [HttpGet("receivable")]
    public async Task<Api_Response> PageReceivable([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.PageReceivableAsync(req));

    [HttpGet("payable")]
    public async Task<Api_Response> PagePayable([FromQuery] Page_Request req)
        => Api_Response.Ok(await _svc.PagePayableAsync(req));

    [HttpPost("cost")]
    [ActiconLog_(group = "财务", action = "成本核算")]
    public async Task<Api_Response> Cost([FromBody] Cost_Calc_Request req) => Api_Response.Ok(await _svc.CalcCostAsync(req));
}

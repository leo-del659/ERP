using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayController : ControllerBase
{
    private readonly I_Pay_Service _svc;
    public PayController(I_Pay_Service svc) => _svc = svc;

    /// <summary>支付渠道回调，允许匿名；内部按 notifyNo 幂等。</summary>
    [AllowAnonymous]
    [HttpPost("notify")]
    public async Task<Api_Response> Notify([FromBody] Pay_Notify_Request req)
        => Api_Response.Ok(await _svc.NotifyAsync(req));
}

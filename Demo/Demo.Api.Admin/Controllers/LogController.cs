using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly I_Log_Service _svc;
    public LogController(I_Log_Service svc) => _svc = svc;

    [HttpGet("oper")]
    public async Task<Api_Response> Oper([FromQuery] Page_Request req) => Api_Response.Ok(await _svc.PageOperAsync(req));

    [HttpGet("ex")]
    public async Task<Api_Response> Ex([FromQuery] Page_Request req) => Api_Response.Ok(await _svc.PageExAsync(req));
}

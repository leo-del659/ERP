using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly I_Dashboard_Service _svc;
    public DashboardController(I_Dashboard_Service svc) => _svc = svc;

    [HttpGet]
    public async Task<Api_Response> Get() => Api_Response.Ok(await _svc.OverviewAsync());
}

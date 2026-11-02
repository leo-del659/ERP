using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 决策报表：库存周转、生产进度、毛利分析、销售趋势、看板 KPI。
/// </summary>
[ApiController]
[Authorize]
[Permission_("/rpt")]
[Route("api/[controller]")]
public class RptController : ControllerBase
{
    private readonly I_Rpt_Service _svc;
    public RptController(I_Rpt_Service svc) => _svc = svc;

    /// <summary>库存周转分析</summary>
    [HttpGet("turnover")]
    public async Task<Api_Response> Turnover([FromQuery] int days = 90)
        => Api_Response.Ok(await _svc.TurnoverAsync(days));

    /// <summary>生产进度</summary>
    [HttpGet("production")]
    public async Task<Api_Response> Production()
        => Api_Response.Ok(await _svc.ProductionProgressAsync());

    /// <summary>毛利分析</summary>
    [HttpGet("margin")]
    public async Task<Api_Response> Margin([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
        => Api_Response.Ok(await _svc.GrossMarginAsync(from, to));

    /// <summary>销售趋势</summary>
    [HttpGet("trend")]
    public async Task<Api_Response> Trend([FromQuery] int months = 12)
        => Api_Response.Ok(await _svc.SalesTrendAsync(months));

    /// <summary>看板 KPI 列表</summary>
    [HttpGet("kpi")]
    public async Task<Api_Response> Kpi([FromQuery] int limit = 50)
        => Api_Response.Ok(await _svc.KpiListAsync(limit));
}

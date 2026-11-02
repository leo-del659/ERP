using System.Security.Claims;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 生产制造业务级联接口（工单→生产→领料→报工）。
/// 各接口带细粒度权限与行为日志（落库 sys_oper_log）。
/// </summary>
[ApiController]
[Authorize]
public class MesBizController : ControllerBase
{
    private readonly IMesService _mes;
    public MesBizController(IMesService mes) { _mes = mes; }

    private string? Operator => User.FindFirst(ClaimTypes.Name)?.Value;

    /// <summary>工单下发生产（生成生产任务）</summary>
    [HttpPost("api/mes/biz/work-order/{id}/dispatch")]
    [Permission_("/mes/work-order")]
    [ActiconLog_Attribute(group = "MES", action = "工单下发生产")]
    public async Task<Api_Response> Dispatch(long id)
    {
        await _mes.DispatchWorkOrderAsync(id, Operator);
        return Api_Response.Ok(null, "下发成功");
    }

    /// <summary>桥接：由生产订单(PrOrder)批量同步生成 MES 工单（避免双工单）</summary>
    [HttpPost("api/mes/biz/work-order/sync-from-pr")]
    [Permission_("/mes/work-order")]
    [ActiconLog_Attribute(group = "MES", action = "同步生产订单为工单")]
    public async Task<Api_Response> SyncFromPr()
    {
        var n = await _mes.SyncWorkOrdersFromPrAsync(Operator);
        return Api_Response.Ok(new { count = n }, n > 0 ? $"已同步 {n} 张生产订单" : "没有可同步的生产订单");
    }

    /// <summary>桥接：将 MES 工单执行结果回写生产订单（状态 + 完工数量）</summary>
    [HttpPost("api/mes/biz/work-order/{id}/sync-pr")]
    [Permission_("/mes/work-order")]
    [ActiconLog_Attribute(group = "MES", action = "回写生产订单状态")]
    public async Task<Api_Response> SyncToPr(long id)
    {
        await _mes.SyncWorkOrderToPrAsync(id, Operator);
        return Api_Response.Ok(null, "已回写生产订单");
    }

    /// <summary>生产任务领料（生成领料单并扣减库存）</summary>
    [HttpPost("api/mes/biz/production-order/{id}/issue")]
    [Permission_("/mes/production-order")]
    [ActiconLog_Attribute(group = "MES", action = "生产领料")]
    public async Task<Api_Response> Issue(long id)
    {
        await _mes.IssueMaterialAsync(id, Operator);
        return Api_Response.Ok(null, "领料成功");
    }

    /// <summary>生产任务报工（完工入库）</summary>
    [HttpPost("api/mes/biz/production-order/{id}/report")]
    [Permission_("/mes/production-order")]
    [ActiconLog_Attribute(group = "MES", action = "生产报工")]
    public async Task<Api_Response> Report(long id)
    {
        await _mes.ReportWorkAsync(id, Operator);
        return Api_Response.Ok(null, "报工成功");
    }

    /// <summary>报工单质量检验（终检）</summary>
    [HttpPost("api/mes/biz/work-report/{id}/qc")]
    [Permission_("/mes/quality-check")]
    [ActiconLog_Attribute(group = "MES", action = "质量检验")]
    public async Task<Api_Response> QualityCheck(long id)
    {
        await _mes.QualityCheckAsync(id, Operator);
        return Api_Response.Ok(null, "质检完成");
    }
}

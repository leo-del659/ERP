using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Service.Interface;

namespace Demo.Service.Interface;

/// <summary>
/// 决策报表服务：库存周转、生产进度、毛利分析、看板 KPI。
/// </summary>
[Provider_Attribute]
public interface I_Rpt_Service
{
    /// <summary>库存周转分析（周转率 / 周转天数 / 呆滞料）</summary>
    Task<object> TurnoverAsync(int days);

    /// <summary>生产进度（在制工单完成率、报工良率、产线汇总）</summary>
    Task<object> ProductionProgressAsync();

    /// <summary>毛利分析（按物料 / 按客户维度的收入、成本、毛利）</summary>
    Task<object> GrossMarginAsync(DateOnly? from, DateOnly? to);

    /// <summary>看板 KPI 列表</summary>
    Task<object> KpiListAsync(int limit);

    /// <summary>销售趋势（按月统计销售额/成本/毛利）</summary>
    Task<object> SalesTrendAsync(int months);
}

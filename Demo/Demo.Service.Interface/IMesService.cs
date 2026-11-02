using System.Threading.Tasks;
using Demo.Infrastructure.Attributes;

namespace Demo.Service.Interface;

/// <summary>
/// 生产制造业务级联服务：工单→生产派工→领料扣库存→报工入库
/// </summary>
[Provider_Attribute]
public interface IMesService
{
    /// <summary>工单下发：生成生产任务，工单进入生产中</summary>
    Task DispatchWorkOrderAsync(long workOrderId, string? operatorName);

    /// <summary>生产领料：生成领料单并扣减库存</summary>
    Task IssueMaterialAsync(long productionOrderId, string? operatorName);

    /// <summary>报工：记录报工单，完工入库（增加产成品库存），更新工单/任务进度</summary>
    Task ReportWorkAsync(long productionOrderId, string? operatorName);

    /// <summary>质量检验：基于报工单生成终检记录</summary>
    Task QualityCheckAsync(long workReportId, string? operatorName);

    /// <summary>
    /// 桥接：由「已下达/生产中」且尚未生成 MES 工单的生产订单(PrOrder)批量生成 MES 工单，
    /// 返回新增数量（避免双工单重复录入）。
    /// </summary>
    Task<int> SyncWorkOrdersFromPrAsync(string? operatorName);

    /// <summary>桥接：将 MES 工单执行结果（状态 + 完工数量）回写生产订单</summary>
    Task SyncWorkOrderToPrAsync(long workOrderId, string? operatorName);
}

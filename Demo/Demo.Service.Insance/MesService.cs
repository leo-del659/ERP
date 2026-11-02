using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Service.Insance;

/// <summary>
/// 生产制造业务级联：工单 → 生产派工 → BOM驱动领料 → 报工入库 → 质量检验。
/// 复用主数据 BOM（md_bom）展开物料需求，并通过 I_Inventory_Service 走库存事务。
/// 与既有生产域（PrOrder / Production_Service）桥接：
///   - MES 工单可由生产订单(PrOrder)生成，避免双工单重复录入；
///   - 派工/报工自动回写生产订单状态与完工数量，保持双向一致。
/// </summary>
[Inject_Attribute]
public class MesService : IMesService
{
    private readonly ErpDbContext _db;
    private readonly I_Inventory_Service _inv;

    public MesService(ErpDbContext db, I_Inventory_Service inv)
    {
        _db = db;
        _inv = inv;
    }

    /// <summary>默认仓库（MES 未建模仓库维度时取首个仓库）</summary>
    private async Task<long> DefaultWarehouseAsync()
    {
        var wh = await _db.MdWarehouses.OrderBy(x => x.Id).FirstOrDefaultAsync();
        return wh?.Id ?? 0;
    }

    /// <summary>工单下发：按工单生成生产任务（派工），工单进入生产中</summary>
    public async Task DispatchWorkOrderAsync(long workOrderId, string? operatorName)
    {
        var wo = await _db.MesWorkOrders.FindAsync(workOrderId)
                 ?? throw new Biz_Exception("工单不存在");
        if (wo.Status != 0) throw new Biz_Exception("仅“待下发”的工单可下发生产");

        // 优先按产品启用的工艺路线首道工序确定工位
        long? wsId = null;
        var routing = await _db.MesRoutings
            .Where(r => r.ProductId == wo.ProductId && r.Status == 1)
            .OrderByDescending(r => r.Version)
            .FirstOrDefaultAsync();
        if (routing != null)
        {
            var step = await _db.MesRoutingSteps
                .Where(s => s.RoutingId == routing.Id)
                .OrderBy(s => s.StepNo)
                .FirstOrDefaultAsync();
            wsId = step?.WorkstationId;
        }
        if (wsId == null)
        {
            var ws = await _db.MesWorkstations.OrderBy(x => x.Id).FirstOrDefaultAsync();
            wsId = ws?.Id;
        }

        _db.MesProductionOrders.Add(new MesProductionOrder
        {
            ProdNo = "PO" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            WorkOrderId = wo.Id,
            WorkstationId = wsId,
            PlanQty = wo.PlanQty,
            DoneQty = 0,
            Status = 1,                 // 生产中
            StartTime = DateTime.Now,
            CreateTime = DateTime.Now
        });

        wo.Status = 1;                  // 生产中
        await SyncToPrAsync(wo);        // 桥接：回写生产订单状态
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// 生产领料：按主数据 BOM(md_bom) 展开物料需求，逐项生成领料单并扣减库存。
    /// 需求量 = 计划量 × 单位用量 × (1 + 损耗率)
    /// </summary>
    public async Task IssueMaterialAsync(long productionOrderId, string? operatorName)
    {
        var po = await _db.MesProductionOrders.FindAsync(productionOrderId)
                 ?? throw new Biz_Exception("生产任务不存在");
        var wo = await _db.MesWorkOrders.FindAsync(po.WorkOrderId)
                 ?? throw new Biz_Exception("关联工单不存在");

        if (po.Status == 0) po.Status = 1;   // 待开工 → 生产中

        // BOM 展开：该产品（父项）下所有启用的子项物料
        var bomItems = await _db.MdBoms
            .Where(b => b.ParentMaterialId == wo.ProductId && b.Status != 0)
            .ToListAsync();
        if (bomItems.Count == 0)
            throw new Biz_Exception($"产品（物料ID={wo.ProductId}）未配置启用的 BOM，无法领料");

        var wh = await DefaultWarehouseAsync();

        await using var tx = await _db.Database.BeginTransactionAsync();
        foreach (var item in bomItems)
        {
            decimal need = wo.PlanQty * item.QtyPerUnit * (1 + (item.LossRate ?? 0));

            _db.MesMaterialIssues.Add(new MesMaterialIssue
            {
                IssueNo = "MI" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                WorkOrderId = wo.Id,
                MaterialId = item.ChildMaterialId,
                Qty = need,
                Status = 1,                 // 已领
                IssueTime = DateTime.Now,
                CreateTime = DateTime.Now
            });

            // 扣减库存（走库存服务，生成库存流水）
            await _inv.ApplyQtyAsync(item.ChildMaterialId, wh, need, -1, "OUT_MES", wo.OrderNo);
        }
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    /// <summary>报工：记录报工单，完工入库（增加产成品库存），更新工单/任务进度</summary>
    public async Task ReportWorkAsync(long productionOrderId, string? operatorName)
    {
        var po = await _db.MesProductionOrders.FindAsync(productionOrderId)
                 ?? throw new Biz_Exception("生产任务不存在");
        var wo = await _db.MesWorkOrders.FindAsync(po.WorkOrderId)
                 ?? throw new Biz_Exception("关联工单不存在");

        int good = po.PlanQty;          // 演示：按计划量产出，全部合格
        var wh = await DefaultWarehouseAsync();

        await using var tx = await _db.Database.BeginTransactionAsync();

        _db.MesWorkReports.Add(new MesWorkReport
        {
            ReportNo = "WR" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            ProductionOrderId = po.Id,
            GoodQty = good,
            BadQty = 0,
            WorkHours = 0,
            Status = 1,                 // 已确认
            ReportTime = DateTime.Now,
            CreateTime = DateTime.Now
        });

        // 更新生产任务进度
        po.DoneQty = po.DoneQty + good;
        po.Status = (sbyte)(po.DoneQty >= po.PlanQty ? 2 : 1);   // 完工 / 生产中

        // 更新工单进度
        wo.DoneQty = wo.DoneQty + good;
        if (wo.DoneQty >= wo.PlanQty) wo.Status = 2;             // 完工

        // 桥接：回写生产订单状态与完工数量
        await SyncToPrAsync(wo);

        // 完工入库：增加产成品库存（走库存服务）
        await _inv.ApplyQtyAsync(wo.ProductId, wh, good, 1, "IN_MES", wo.OrderNo);

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    /// <summary>质量检验：基于报工单生成终检记录</summary>
    public async Task QualityCheckAsync(long workReportId, string? operatorName)
    {
        var report = await _db.MesWorkReports.FindAsync(workReportId)
                     ?? throw new Biz_Exception("报工单不存在");

        _db.MesQualityChecks.Add(new MesQualityCheck
        {
            CheckNo = "QC" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            ProductionOrderId = report.ProductionOrderId,
            WorkReportId = report.Id,
            CheckType = 3,                                  // 终检
            Qty = report.GoodQty + report.BadQty,
            PassQty = report.GoodQty,
            FailQty = report.BadQty,
            Result = (sbyte)(report.BadQty > 0 ? 2 : 1),    // 不合格 / 合格
            CheckTime = DateTime.Now,
            CreateTime = DateTime.Now
        });

        await _db.SaveChangesAsync();
    }

    // ==================================================================
    //  与既有生产域（PrOrder）的桥接：避免双工单
    // ==================================================================

    /// <summary>
    /// 由「已下达/生产中」且尚未生成 MES 工单的生产订单(PrOrder)批量生成 MES 工单，
    /// 沿用生产制令号保持双向可追溯，避免两套工单重复录入。
    /// </summary>
    public async Task<int> SyncWorkOrdersFromPrAsync(string? operatorName)
    {
        var linked = await _db.MesWorkOrders
            .Where(w => w.PrOrderId != null)
            .Select(w => w.PrOrderId!.Value)
            .ToListAsync();

        var orders = await _db.PrOrders
            .Where(o => (o.Status == Order_State_Machine.Pr.Released
                         || o.Status == Order_State_Machine.Pr.Producing)
                        && !linked.Contains(o.Id))
            .ToListAsync();

        foreach (var o in orders)
        {
            _db.MesWorkOrders.Add(new MesWorkOrder
            {
                OrderNo = o.OrderNo,             // 沿用制令号，双向可追溯
                ProductId = o.MaterialId,
                PlanQty = (int)o.PlanQty,
                DoneQty = 0,
                Priority = 1,
                Status = 0,                      // 待下发
                PlanStart = o.StartDate,
                PlanEnd = o.EndDate,
                PrOrderId = o.Id,
                Source = 2,                      // 由生产订单生成
                Remark = "由生产订单自动生成",
                CreateTime = DateTime.Now
            });
        }

        if (orders.Count > 0) await _db.SaveChangesAsync();
        return orders.Count;
    }

    /// <summary>桥接：将 MES 工单执行结果（状态 + 完工数量）回写生产订单</summary>
    public async Task SyncWorkOrderToPrAsync(long workOrderId, string? operatorName)
    {
        var wo = await _db.MesWorkOrders.FindAsync(workOrderId)
                 ?? throw new Biz_Exception("工单不存在");
        if (wo.PrOrderId == null)
            throw new Biz_Exception("该工单未关联生产订单，请先由生产订单同步生成");

        await SyncToPrAsync(wo);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// MES 工单状态 → 生产订单状态（仅当状态机允许时迁移），并回写完工数量。
    /// MES: 0待下发/1生产中/2完工  →  PR: 1下达/2生产中/3完工
    /// </summary>
    private async Task SyncToPrAsync(MesWorkOrder wo)
    {
        if (wo.PrOrderId == null) return;
        var pr = await _db.PrOrders.FindAsync(wo.PrOrderId.Value);
        if (pr == null) return;

        sbyte target = wo.Status switch
        {
            1 => Order_State_Machine.Pr.Producing,   // MES 生产中 → PR 生产中
            2 => Order_State_Machine.Pr.Done,        // MES 完工   → PR 完工
            _ => pr.Status ?? Order_State_Machine.Pr.Plan
        };

        if (target != (pr.Status ?? -1)
            && Order_State_Machine.Pr.Map.CanTransit(pr.Status, target))
        {
            pr.Status = Order_State_Machine.Pr.Transit(pr.Status, target);
        }

        pr.CompletedQty = wo.DoneQty;
    }
}

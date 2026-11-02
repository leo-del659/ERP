using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Service.Instance;

[Inject_Attribute]
public class Production_Service : I_Production_Service
{
    private readonly ErpDbContext _db;
    private readonly BillNo_Helper _no;
    private readonly I_Inventory_Service _inv;

    public Production_Service(ErpDbContext db, BillNo_Helper no, I_Inventory_Service inv)
    {
        _db = db;
        _no = no;
        _inv = inv;
    }

    public async Task<object> CreateOrderAsync(Pr_Order_Create_Request req)
    {
        var order = new PrOrder
        {
            OrderNo = await _no.NextAsync("MO"),
            MaterialId = req.materialId,
            PlanQty = req.planQty,
            CompletedQty = 0,
            StartDate = req.startDate,
            EndDate = req.endDate,
            Status = Order_State_Machine.Pr.Plan,
            SourceSoId = req.sourceSoId,
            SourceMpsId = req.sourceMpsId,
            CreateTime = DateTime.Now
        };
        _db.PrOrders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<object> ReleaseAsync(long id)
    {
        var order = await _db.PrOrders.FindAsync(id) ?? throw new Biz_Exception("生产订单不存在");
        order.Status = Order_State_Machine.Pr.Transit(order.Status, Order_State_Machine.Pr.Released);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<object> IssueAsync(Issue_Request req)
    {
        var order = await _db.PrOrders.FindAsync(req.prOrderId) ?? throw new Biz_Exception("生产订单不存在");
        if (order.Status is 0) throw new Biz_Exception("请先下达生产订单");

        await using var tx = await _db.Database.BeginTransactionAsync();
        if (order.Status == Order_State_Machine.Pr.Released)
            order.Status = Order_State_Machine.Pr.Transit(order.Status, Order_State_Machine.Pr.Producing);

        _db.PrMaterialIssues.Add(new PrMaterialIssue
        {
            PrOrderId = order.Id,
            MaterialId = req.materialId,
            IssueQty = req.issueQty,
            IssueTime = DateTime.Now
        });
        await _inv.ApplyQtyAsync(req.materialId, req.warehouseId, req.issueQty, -1, "OUT_PR", order.OrderNo);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return order;
    }

    public async Task<object> ReportAsync(Report_Request req)
    {
        var order = await _db.PrOrders.FindAsync(req.prOrderId) ?? throw new Biz_Exception("生产订单不存在");
        _db.PrReports.Add(new PrReport
        {
            PrOrderId = order.Id,
            ProcessName = req.processName,
            ReportQty = req.reportQty,
            GoodQty = req.goodQty,
            BadQty = req.badQty,
            WorkHours = req.workHours,
            ReportTime = DateTime.Now
        });
        await _db.SaveChangesAsync();
        return req;
    }

    public async Task<object> CompleteAsync(long id, long warehouseId)
    {
        var order = await _db.PrOrders.FindAsync(id) ?? throw new Biz_Exception("生产订单不存在");
        var good = await _db.PrReports.Where(d => d.PrOrderId == id).SumAsync(d => d.GoodQty ?? 0);
        if (good <= 0) throw new Biz_Exception("没有良品报工，不能完工入库");

        await using var tx = await _db.Database.BeginTransactionAsync();
        await _inv.ApplyQtyAsync(order.MaterialId, warehouseId, good, 1, "IN_PR", order.OrderNo);
        order.CompletedQty = good;
        order.Status = Order_State_Machine.Pr.Transit(order.Status, Order_State_Machine.Pr.Done);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return order;
    }

    public async Task<object> RunMrpAsync()
    {
        var mpsList = await _db.PrMps.Where(d => d.Status == 1).ToListAsync();
        var mrpNo = await _no.NextAsync("MRP");
        var results = new List<PrMrp>();

        foreach (var mps in mpsList)
        {
            await ExplodeAsync(mrpNo, mps.MaterialId, mps.PlanQty, mps.PlanStartDate, results, new HashSet<long>());
            mps.Status = 2;
        }

        _db.PrMrps.AddRange(results);
        await _db.SaveChangesAsync();
        return results;
    }

    private async Task ExplodeAsync(string mrpNo, long materialId, decimal qty, DateOnly? date, List<PrMrp> bag, HashSet<long> path)
    {
        if (!path.Add(materialId)) return;
        var mat = await _db.MdMaterials.FindAsync(materialId);
        var onHand = await _db.InvStocks.Where(d => d.MaterialId == materialId).SumAsync(d => (decimal?)d.CurrentQty) ?? 0;
        var safety = mat?.SafetyStock ?? 0;
        var net = qty + safety - onHand;
        if (net < 0) net = 0;

        var children = await _db.MdBoms.Where(d => d.ParentMaterialId == materialId && d.Status != 0).ToListAsync();
        var suggest = children.Count > 0 ? "MAKE" : "BUY";
        bag.Add(new PrMrp
        {
            MrpNo = mrpNo,
            MaterialId = materialId,
            GrossRequirement = qty,
            OnHandQty = onHand,
            OnOrderQty = 0,
            SafetyStock = safety,
            NetRequirement = net,
            SuggestType = suggest,
            SuggestQty = net,
            SuggestDate = date,
            Status = 0,
            CreateTime = DateTime.Now
        });

        foreach (var bom in children)
        {
            var childQty = qty * bom.QtyPerUnit * (1 + (bom.LossRate ?? 0));
            await ExplodeAsync(mrpNo, bom.ChildMaterialId, childQty, date, bag, path);
        }
        path.Remove(materialId);
    }

    public async Task<object> PageAsync(Page_Request req)
    {
        // 联物料表，补产品名称
        var q = from o in _db.PrOrders
                join m in _db.MdMaterials on o.MaterialId equals m.Id
                select new
                {
                    o.Id, o.OrderNo, o.MaterialId,
                    MaterialCode = m.MaterialCode, MaterialName = m.MaterialName,
                    o.PlanQty, o.CompletedQty, o.Status, o.CreateTime
                };
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.OrderNo.Contains(k) || (d.MaterialName ?? "").Contains(k));
        }
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id).Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    // ==================================================================
    //  主生产计划 MPS
    // ==================================================================

    /// <summary>新建主生产计划（MPS）：草稿状态，确认后才参与 MRP 运算。</summary>
    public async Task<object> CreateMpsAsync(Mps_Create_Request req)
    {
        if (req.planQty <= 0) throw new Biz_Exception("计划数量必须大于0");
        var mps = new PrMp
        {
            MpsNo = await _no.NextAsync("MPS"),
            MaterialId = req.materialId,
            PlanQty = req.planQty,
            PlanStartDate = req.planStartDate,
            PlanEndDate = req.planEndDate,
            SourceType = req.sourceType,
            SourceId = req.sourceId,
            Status = 0,
            CreateTime = DateTime.Now
        };
        _db.PrMps.Add(mps);
        await _db.SaveChangesAsync();
        return mps;
    }

    public async Task<object> MpsPageAsync(Page_Request req)
    {
        var q = from m in _db.PrMps
                join mat in _db.MdMaterials on m.MaterialId equals mat.Id
                select new
                {
                    m.Id, m.MpsNo, m.MaterialId,
                    MaterialCode = mat.MaterialCode,
                    MaterialName = mat.MaterialName,
                    m.PlanQty, m.PlanStartDate, m.PlanEndDate,
                    m.SourceType, m.SourceId, m.Status, m.CreateTime
                };
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => d.MpsNo.Contains(req.keyword) || d.MaterialCode.Contains(req.keyword)
                         || d.MaterialName.Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    /// <summary>确认 MPS：草稿 → 已确认，确认后才可参与 MRP 运算。</summary>
    public async Task<object> ConfirmMpsAsync(long id)
    {
        var mps = await _db.PrMps.FindAsync(id) ?? throw new Biz_Exception("主生产计划不存在");
        if (mps.Status != 0) throw new Biz_Exception("仅草稿状态的计划可确认");
        mps.Status = 1;
        await _db.SaveChangesAsync();
        return mps;
    }

    /// <summary>
    /// MPS 转生产订单：按计划量下达工单，状态 已确认 → 已转生产订单。
    /// </summary>
    public async Task<object> MpsToOrderAsync(Mps_To_Order_Request req)
    {
        var mps = await _db.PrMps.FindAsync(req.mpsId) ?? throw new Biz_Exception("主生产计划不存在");
        if (mps.Status != 1) throw new Biz_Exception("仅已确认的计划可转生产订单");

        var order = await CreateOrderAsync(new Pr_Order_Create_Request
        {
            materialId = mps.MaterialId,
            planQty = mps.PlanQty,
            startDate = mps.PlanStartDate,
            endDate = mps.PlanEndDate,
            sourceMpsId = mps.Id
        });

        mps.Status = 2;
        await _db.SaveChangesAsync();
        return order;
    }

    // ==================================================================
    //  MRP 运算结果
    // ==================================================================

    public async Task<object> MrpPageAsync(Page_Request req, string? mrpNo)
    {
        var q = from r in _db.PrMrps
                join m in _db.MdMaterials on r.MaterialId equals m.Id
                select new
                {
                    r.Id, r.MrpNo, r.MaterialId,
                    MaterialCode = m.MaterialCode,
                    MaterialName = m.MaterialName,
                    r.GrossRequirement, r.OnHandQty, r.OnOrderQty, r.SafetyStock,
                    r.NetRequirement, r.SuggestType, r.SuggestQty, r.SuggestDate,
                    r.Status, r.CreateTime
                };
        if (!string.IsNullOrWhiteSpace(mrpNo)) q = q.Where(d => d.MrpNo == mrpNo);
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => d.MrpNo.Contains(req.keyword) || d.MaterialCode.Contains(req.keyword)
                         || d.MaterialName.Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    /// <summary>
    /// MRP 建议转采购订单：为「建议采购」项生成采购订单，并把该建议置为已转单。
    /// 单价优先取供应商报价，无报价则取最近一次采购成交价。
    /// </summary>
    public async Task<object> MrpToPoAsync(Mrp_To_Po_Request req)
    {
        var mrp = await _db.PrMrps.FindAsync(req.mrpId) ?? throw new Biz_Exception("MRP 建议不存在");
        if (mrp.Status == 1) throw new Biz_Exception("该建议已转单");
        if (mrp.SuggestType != "BUY") throw new Biz_Exception("仅「建议采购」项可转采购订单");

        var qty = mrp.SuggestQty ?? 0;
        if (qty <= 0) throw new Biz_Exception("建议数量必须大于0");

        var unitPrice = 0m;
        var quote = await _db.PoSupplierQuotes
            .Where(d => d.MaterialId == mrp.MaterialId && d.SupplierId == req.supplierId)
            .OrderBy(d => d.Id)
            .FirstOrDefaultAsync();
        if (quote != null)
            unitPrice = quote.QuotePrice;
        else
        {
            var last = await _db.PoOrderDetails
                .Where(d => d.MaterialId == mrp.MaterialId)
                .OrderByDescending(d => d.Id)
                .Select(d => d.UnitPrice)
                .FirstOrDefaultAsync();
            unitPrice = last;
        }

        var po = new PoOrder
        {
            OrderNo = await _no.NextAsync("PO"),
            SupplierId = req.supplierId,
            OrderDate = req.orderDate ?? DateOnly.FromDateTime(DateTime.Today),
            Status = Order_State_Machine.Po.Pending,
            TotalAmount = qty * unitPrice,
            CreatedAt = DateTime.Now
        };
        _db.PoOrders.Add(po);
        await _db.SaveChangesAsync();

        _db.PoOrderDetails.Add(new PoOrderDetail
        {
            OrderId = po.Id,
            MaterialId = mrp.MaterialId,
            Quantity = qty,
            UnitPrice = unitPrice,
            ReceivedQty = 0
        });

        mrp.Status = 1;
        await _db.SaveChangesAsync();
        return po;
    }
}

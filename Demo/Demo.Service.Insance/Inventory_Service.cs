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
public class Inventory_Service : I_Inventory_Service
{
    private readonly ErpDbContext _db;
    private readonly BillNo_Helper _no;
    private readonly RabbitMq_Bus _mq;
    private readonly Redis_Cache _cache;

    public Inventory_Service(ErpDbContext db, BillNo_Helper no, RabbitMq_Bus mq, Redis_Cache cache)
    {
        _db = db;
        _no = no;
        _mq = mq;
        _cache = cache;
    }

    public async Task<object> ConfirmInAsync(Inv_In_Request req)
    {
        if (req.lines.Count == 0) throw new Biz_Exception("入库明细不能为空");
        await using var tx = await _db.Database.BeginTransactionAsync();
        var bill = new InvInOrder
        {
            InNo = await _no.NextAsync("IN"),
            InType = req.inType,
            SourceNo = req.sourceNo,
            WarehouseId = req.warehouseId,
            InDate = req.inDate,
            Status = 1,
            CreateTime = DateTime.Now
        };
        _db.InvInOrders.Add(bill);
        await _db.SaveChangesAsync();

        foreach (var line in req.lines)
        {
            _db.InvInDetails.Add(new InvInDetail { InOrderId = bill.Id, MaterialId = line.materialId, Quantity = line.quantity });
            await ApplyQtyAsync(line.materialId, req.warehouseId, line.quantity, 1, req.inType switch
            {
                "PO" => "IN_PO",
                "PR" => "IN_PR",
                _ => "IN_OTHER"
            }, bill.InNo, line.batchNo);
        }
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        await _cache.RemoveAsync("erp:stock:list");
        return bill;
    }

    public async Task<object> ConfirmOutAsync(Inv_Out_Request req)
    {
        if (req.lines.Count == 0) throw new Biz_Exception("出库明细不能为空");
        await using var tx = await _db.Database.BeginTransactionAsync();
        var bill = new InvOutOrder
        {
            OutNo = await _no.NextAsync("OUT"),
            OutType = req.outType,
            SourceNo = req.sourceNo,
            WarehouseId = req.warehouseId,
            OutDate = req.outDate,
            Status = 1,
            CreateTime = DateTime.Now
        };
        _db.InvOutOrders.Add(bill);
        await _db.SaveChangesAsync();

        foreach (var line in req.lines)
        {
            _db.InvOutDetails.Add(new InvOutDetail { OutOrderId = bill.Id, MaterialId = line.materialId, Quantity = line.quantity });
            await ApplyQtyAsync(line.materialId, req.warehouseId, line.quantity, -1, req.outType switch
            {
                "SO" => "OUT_SO",
                "PR" => "OUT_PR",
                _ => "OUT_OTHER"
            }, bill.OutNo, line.batchNo);
        }
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        await _cache.RemoveAsync("erp:stock:list");
        return bill;
    }

    public async Task<object> TransferAsync(InvTransfer bill)
    {
        if (bill.Quantity <= 0) throw new Biz_Exception("调拨数量必须大于0");
        await using var tx = await _db.Database.BeginTransactionAsync();
        bill.TransferNo = await _no.NextAsync("TR");
        bill.Status = 1;
        _db.InvTransfers.Add(bill);
        await ApplyQtyAsync(bill.MaterialId, bill.FromWarehouseId, bill.Quantity, -1, "ADJ", bill.TransferNo);
        await ApplyQtyAsync(bill.MaterialId, bill.ToWarehouseId, bill.Quantity, 1, "ADJ", bill.TransferNo);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return bill;
    }

    /// <summary>
    /// 库存扣减一致性：同一事务内对台账行 FOR UPDATE，再用 CAS 更新数量，同时写流水。
    /// </summary>
    public async Task ApplyQtyAsync(long materialId, long warehouseId, decimal qty, sbyte direction, string transType, string sourceNo, string? batchNo = null)
    {
        if (qty <= 0) throw new Biz_Exception("数量必须大于0");

        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"INSERT INTO inv_stock(material_id, warehouse_id, current_qty, locked_qty, last_update) SELECT {materialId}, {warehouseId}, 0, 0, NOW() FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM inv_stock WHERE material_id={materialId} AND warehouse_id={warehouseId})");

        var stock = await _db.InvStocks
            .FromSqlInterpolated($"SELECT * FROM inv_stock WHERE material_id={materialId} AND warehouse_id={warehouseId} FOR UPDATE")
            .AsTracking()
            .FirstOrDefaultAsync() ?? throw new Biz_Exception("库存台账锁定失败");

        var before = stock.CurrentQty;
        var after = before + direction * qty;
        if (after < 0) throw new Biz_Exception($"库存不足，物料 {materialId} 可用 {before}，需要 {qty}");

        var affected = await _db.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE inv_stock SET current_qty={after}, last_update=NOW() WHERE id={stock.Id} AND current_qty={before}");
        if (affected != 1) throw new Biz_Exception("库存并发冲突，请重试");

        stock.CurrentQty = after;
        stock.LastUpdate = DateTime.Now;

        _db.InvTransactions.Add(new InvTransaction
        {
            TransNo = await _no.NextAsync("TX"),
            MaterialId = materialId,
            WarehouseId = warehouseId,
            TransType = transType,
            Direction = direction,
            Quantity = qty,
            BeforeQty = before,
            AfterQty = after,
            SourceNo = sourceNo,
            TransTime = DateTime.Now
        });

        // 批次联动：传入批次号时同步登记批次库存（入库增加 / 出库扣减）
        if (!string.IsNullOrWhiteSpace(batchNo))
        {
            var batch = await _db.InvBatches.FirstOrDefaultAsync(d =>
                d.MaterialId == materialId && d.WarehouseId == warehouseId && d.BatchNo == batchNo);
            if (batch == null)
            {
                if (direction > 0)
                {
                    _db.InvBatches.Add(new InvBatch
                    {
                        MaterialId = materialId,
                        WarehouseId = warehouseId,
                        BatchNo = batchNo,
                        Quantity = qty
                    });
                }
            }
            else
            {
                var batchQty = (batch.Quantity ?? 0) + direction * qty;
                if (batchQty < 0) batchQty = 0;   // 批次余量不足时归零，避免负批次
                batch.Quantity = batchQty;
            }
        }

        var mat = await _db.MdMaterials.FindAsync(materialId);
        if (mat?.SafetyStock != null && after < mat.SafetyStock.Value)
        {
            var msg = new Stock_Alert_Message
            {
                MessageId = Guid.NewGuid().ToString("N"),
                MaterialId = materialId,
                MaterialCode = mat.MaterialCode,
                MaterialName = mat.MaterialName,
                WarehouseId = warehouseId,
                CurrentQty = after,
                SafetyStock = mat.SafetyStock.Value
            };
            _mq.Publish(RabbitMq_Bus.StockAlertQueue, msg);
        }
    }

    public async Task<object> StockPageAsync(Page_Request req, long? warehouseId)
    {
        var q = from s in _db.InvStocks
                join m in _db.MdMaterials on s.MaterialId equals m.Id
                join w in _db.MdWarehouses on s.WarehouseId equals w.Id
                select new { s.Id, s.MaterialId, m.MaterialCode, m.MaterialName, m.SafetyStock, s.WarehouseId, w.WarehouseName, s.CurrentQty, s.LockedQty, s.LastUpdate };
        if (warehouseId.HasValue) q = q.Where(d => d.WarehouseId == warehouseId);
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => d.MaterialCode.Contains(req.keyword) || d.MaterialName.Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderBy(d => d.MaterialCode).Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    public async Task<object> TransPageAsync(Page_Request req)
    {
        var q = _db.InvTransactions.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.TransNo.Contains(k) || (d.SourceNo ?? "").Contains(k)
                || _db.MdMaterials.Any(m => m.Id == d.MaterialId && m.MaterialName.Contains(k))
                || _db.MdWarehouses.Any(w => w.Id == d.WarehouseId && w.WarehouseName.Contains(k)));
        }
        var total = await q.CountAsync();
        var rows = await (from d in q
                          join m in _db.MdMaterials on d.MaterialId equals m.Id into mg
                          from m in mg.DefaultIfEmpty()
                          join w in _db.MdWarehouses on d.WarehouseId equals w.Id into wg
                          from w in wg.DefaultIfEmpty()
                          orderby d.Id descending
                          select new
                          {
                              d.Id, d.TransNo, d.MaterialId, d.WarehouseId, d.TransType, d.Direction,
                              d.Quantity, d.BeforeQty, d.AfterQty, d.SourceNo, d.TransTime,
                              MaterialName = m.MaterialName ?? "",
                              WarehouseName = w.WarehouseName ?? ""
                          })
                         .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    public async Task<object> AlertsAsync()
    {
        var rows = await (from s in _db.InvStocks
                          join m in _db.MdMaterials on s.MaterialId equals m.Id
                          join w in _db.MdWarehouses on s.WarehouseId equals w.Id
                          where m.SafetyStock != null && s.CurrentQty < m.SafetyStock
                          select new { m.MaterialCode, m.MaterialName, s.WarehouseId, s.CurrentQty, m.SafetyStock, WarehouseName = w.WarehouseName }).ToListAsync();
        return rows;
    }

    // ==================================================================
    //  库存盘点
    // ==================================================================

    /// <summary>
    /// 创建盘点单：按当前台账快照生成账面数量，作为盘点基准。
    /// </summary>
    public async Task<object> CreateCheckAsync(Check_Create_Request req)
    {
        if (req.warehouseId <= 0) throw new Biz_Exception("请选择盘点仓库");

        var wh = await _db.MdWarehouses.FindAsync(req.warehouseId) ?? throw new Biz_Exception("仓库不存在");
        var bill = new InvCheck
        {
            CheckNo = await _no.NextAsync("CK"),
            WarehouseId = req.warehouseId,
            CheckDate = req.checkDate,
            Status = Order_State_Machine.StockCheck.Checking
        };
        _db.InvChecks.Add(bill);
        await _db.SaveChangesAsync();

        // 快照账面数量：materialIds 为空则盘点该仓库全部在库物料
        var stockQuery = _db.InvStocks.AsQueryable().Where(d => d.WarehouseId == req.warehouseId);
        if (req.materialIds is { Count: > 0 })
            stockQuery = stockQuery.Where(d => req.materialIds!.Contains(d.MaterialId));

        var stocks = await stockQuery.ToListAsync();
        foreach (var s in stocks)
        {
            _db.InvCheckDetails.Add(new InvCheckDetail
            {
                CheckId = bill.Id,
                MaterialId = s.MaterialId,
                BookQty = s.CurrentQty,
                ActualQty = null,
                DiffQty = null
            });
        }
        await _db.SaveChangesAsync();
        return bill;
    }

    public async Task<object> CheckPageAsync(Page_Request req)
    {
        var q = _db.InvChecks.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.CheckNo.Contains(k)
                || _db.MdWarehouses.Any(w => w.Id == d.WarehouseId && w.WarehouseName.Contains(k)));
        }
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    public async Task<object> CheckDetailAsync(long checkId)
    {
        var bill = await _db.InvChecks.FindAsync(checkId) ?? throw new Biz_Exception("盘点单不存在");
        var details = await (from d in _db.InvCheckDetails
                             join m in _db.MdMaterials on d.MaterialId equals m.Id
                             where d.CheckId == checkId
                             select new
                             {
                                 d.Id,
                                 d.MaterialId,
                                 MaterialCode = m.MaterialCode,
                                 MaterialName = m.MaterialName,
                                 d.BookQty,
                                 d.ActualQty,
                                 d.DiffQty
                             }).ToListAsync();
        return new { check = bill, details };
    }

    /// <summary>
    /// 录入实盘数量：计算差异量，状态 盘点中 → 已完成。
    /// </summary>
    public async Task<object> FinishCheckAsync(Check_Finish_Request req)
    {
        var bill = await _db.InvChecks.FindAsync(req.checkId) ?? throw new Biz_Exception("盘点单不存在");
        bill.Status = Order_State_Machine.StockCheck.Transit(bill.Status, Order_State_Machine.StockCheck.Finished);

        var details = await _db.InvCheckDetails.Where(d => d.CheckId == req.checkId).ToListAsync();
        foreach (var line in req.lines)
        {
            var dtl = details.FirstOrDefault(d => d.MaterialId == line.materialId)
                      ?? throw new Biz_Exception($"物料 {line.materialId} 不在本次盘点范围内");
            dtl.ActualQty = line.actualQty;
            dtl.DiffQty = line.actualQty - (dtl.BookQty ?? 0);
        }
        await _db.SaveChangesAsync();
        return bill;
    }

    /// <summary>
    /// 审核盘点单：按差异量调整库存（盘盈入 / 盘亏出），生成库存流水。
    /// 调整走统一的 ApplyQtyAsync，保证行锁 + CAS + 流水的一致性语义。
    /// </summary>
    public async Task<object> AuditCheckAsync(long checkId)
    {
        var bill = await _db.InvChecks.FindAsync(checkId) ?? throw new Biz_Exception("盘点单不存在");
        bill.Status = Order_State_Machine.StockCheck.Transit(bill.Status, Order_State_Machine.StockCheck.Audited);

        var details = await _db.InvCheckDetails.Where(d => d.CheckId == checkId).ToListAsync();
        await using var tx = await _db.Database.BeginTransactionAsync();

        foreach (var dtl in details)
        {
            var diff = dtl.DiffQty ?? 0;
            if (diff == 0) continue;
            // diff > 0 盘盈（入库），diff < 0 盘亏（出库）
            await ApplyQtyAsync(dtl.MaterialId, bill.WarehouseId, Math.Abs(diff),
                diff > 0 ? (sbyte)1 : (sbyte)-1, "ADJ", bill.CheckNo);
        }

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        await _cache.RemoveAsync("erp:stock:list");
        return bill;
    }

    /// <summary>
    /// 盘点审批驳回：状态退回「盘点中」，清除已录入的实盘数，允许修改后重新送审。
    /// 不产生任何库存调整。
    /// </summary>
    public async Task<object> RejectCheckAsync(long checkId)
    {
        var bill = await _db.InvChecks.FindAsync(checkId) ?? throw new Biz_Exception("盘点单不存在");
        if (bill.Status == Order_State_Machine.StockCheck.Finished)
            bill.Status = Order_State_Machine.StockCheck.Transit(bill.Status, Order_State_Machine.StockCheck.Checking);

        var details = await _db.InvCheckDetails.Where(d => d.CheckId == checkId).ToListAsync();
        foreach (var d in details)
        {
            d.ActualQty = null;
            d.DiffQty = null;
        }
        await _db.SaveChangesAsync();
        return bill;
    }

    // ==================================================================
    //  批次库存
    // ==================================================================

    public async Task<object> BatchPageAsync(Page_Request req, long? warehouseId)
    {
        var q = from b in _db.InvBatches
                join m in _db.MdMaterials on b.MaterialId equals m.Id
                join w in _db.MdWarehouses on b.WarehouseId equals w.Id
                select new
                {
                    b.Id, b.MaterialId, m.MaterialCode, m.MaterialName,
                    b.WarehouseId, w.WarehouseName,
                    b.BatchNo, b.Quantity, b.ProductionDate, b.ExpiryDate
                };
        if (warehouseId.HasValue) q = q.Where(d => d.WarehouseId == warehouseId);
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => d.MaterialCode.Contains(req.keyword) || d.MaterialName.Contains(req.keyword)
                         || d.BatchNo.Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    public async Task<object> SaveBatchAsync(Batch_Save_Request req)
    {
        if (string.IsNullOrWhiteSpace(req.batchNo)) throw new Biz_Exception("批次号不能为空");
        var batch = await _db.InvBatches.FirstOrDefaultAsync(d =>
            d.MaterialId == req.materialId && d.WarehouseId == req.warehouseId && d.BatchNo == req.batchNo);
        if (batch == null)
        {
            batch = new InvBatch
            {
                MaterialId = req.materialId,
                WarehouseId = req.warehouseId,
                BatchNo = req.batchNo,
                Quantity = req.quantity,
                ProductionDate = req.productionDate,
                ExpiryDate = req.expiryDate
            };
            _db.InvBatches.Add(batch);
        }
        else
        {
            batch.Quantity = (batch.Quantity ?? 0) + req.quantity;
            if (req.productionDate.HasValue) batch.ProductionDate = req.productionDate;
            if (req.expiryDate.HasValue) batch.ExpiryDate = req.expiryDate;
        }
        await _db.SaveChangesAsync();
        return batch;
    }
}

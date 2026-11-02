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
public class Purchase_Service : I_Purchase_Service
{
    private readonly ErpDbContext _db;
    private readonly BillNo_Helper _no;
    private readonly I_Inventory_Service _inv;

    public Purchase_Service(ErpDbContext db, BillNo_Helper no, I_Inventory_Service inv)
    {
        _db = db;
        _no = no;
        _inv = inv;
    }

    public async Task<object> CreateAsync(Po_Create_Request req)
    {
        if (req.lines.Count == 0) throw new Biz_Exception("采购明细不能为空");
        var order = new PoOrder
        {
            OrderNo = await _no.NextAsync("PO"),
            SupplierId = req.supplierId,
            OrderDate = req.orderDate,
            Status = Order_State_Machine.Po.Pending,
            TotalAmount = req.lines.Sum(x => x.quantity * x.unitPrice),
            CreatedAt = DateTime.Now
        };
        _db.PoOrders.Add(order);
        await _db.SaveChangesAsync();
        foreach (var line in req.lines)
        {
            _db.PoOrderDetails.Add(new PoOrderDetail
            {
                OrderId = order.Id,
                MaterialId = line.materialId,
                Quantity = line.quantity,
                UnitPrice = line.unitPrice,
                ReceivedQty = 0
            });
        }
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<object> ApproveAsync(long id)
    {
        var order = await _db.PoOrders.FindAsync(id) ?? throw new Biz_Exception("采购订单不存在");
        order.Status = Order_State_Machine.Po.Transit(order.Status, Order_State_Machine.Po.Approved);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<object> ReceiveAsync(Receipt_Create_Request req)
    {
        var order = await _db.PoOrders.FindAsync(req.orderId) ?? throw new Biz_Exception("采购订单不存在");
        if (order.Status is not (1 or 2)) throw new Biz_Exception("仅已审/部分收货订单可收货");

        var receipt = new PoReceipt
        {
            ReceiptNo = await _no.NextAsync("RN"),
            OrderId = order.Id,
            SupplierId = order.SupplierId,
            ReceiptDate = req.receiptDate,
            Status = 0,
            CreateTime = DateTime.Now
        };
        _db.PoReceipts.Add(receipt);
        await _db.SaveChangesAsync();
        var details = await _db.PoOrderDetails.Where(d => d.OrderId == order.Id).ToListAsync();
        foreach (var line in req.lines)
        {
            var dtl = details.FirstOrDefault(x => x.MaterialId == line.materialId) ?? throw new Biz_Exception("收货物料不在订单中");
            if ((dtl.ReceivedQty ?? 0) + line.quantity > dtl.Quantity) throw new Biz_Exception("超订单数量收货");
            _db.PoReceiptDetails.Add(new PoReceiptDetail
            {
                ReceiptId = receipt.Id,
                MaterialId = line.materialId,
                Quantity = line.quantity,
                QualifiedQty = 0
            });
        }
        await _db.SaveChangesAsync();
        return receipt;
    }

    public async Task<object> InspectAsync(long receiptId, bool pass)
    {
        var receipt = await _db.PoReceipts.FindAsync(receiptId) ?? throw new Biz_Exception("收货单不存在");
        receipt.Status = (sbyte)(pass ? 1 : 2);
        if (!pass)
        {
            await _db.SaveChangesAsync();
            return receipt;
        }

        var order = await _db.PoOrders.FindAsync(receipt.OrderId) ?? throw new Biz_Exception("采购订单不存在");
        var rdetails = await _db.PoReceiptDetails.Where(d => d.ReceiptId == receipt.Id).ToListAsync();
        var odetails = await _db.PoOrderDetails.Where(d => d.OrderId == order.Id).ToListAsync();

        await using var tx = await _db.Database.BeginTransactionAsync();
        var warehouseId = await _db.MdWarehouses.Select(d => d.Id).FirstOrDefaultAsync();
        if (warehouseId == 0) throw new Biz_Exception("请先维护仓库档案");

        foreach (var line in rdetails)
        {
            line.QualifiedQty = line.Quantity;
            var od = odetails.First(x => x.MaterialId == line.MaterialId);
            od.ReceivedQty = (od.ReceivedQty ?? 0) + line.Quantity;
            await _inv.ApplyQtyAsync(line.MaterialId, warehouseId, line.Quantity, 1, "IN_PO", receipt.ReceiptNo);
        }

        var all = odetails.All(x => (x.ReceivedQty ?? 0) >= x.Quantity);
        if (all)
            order.Status = Order_State_Machine.Po.Transit(order.Status, Order_State_Machine.Po.Completed);
        else if (order.Status == Order_State_Machine.Po.Approved)
            order.Status = Order_State_Machine.Po.Transit(order.Status, Order_State_Machine.Po.Partial);

        _db.FinPayables.Add(new FinPayable
        {
            PayNo = await _no.NextAsync("AP"),
            SupplierId = order.SupplierId,
            SourceOrderId = order.Id,
            Amount = rdetails.Sum(x =>
            {
                var od = odetails.First(t => t.MaterialId == x.MaterialId);
                return x.Quantity * od.UnitPrice;
            }),
            PaidAmount = 0,
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
            Status = 0,
            CreateTime = DateTime.Now
        });

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return receipt;
    }

    public async Task<object> PageAsync(Page_Request req)
    {
        var q = _db.PoOrders.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.OrderNo.Contains(k)
                || _db.MdSuppliers.Any(s => s.Id == d.SupplierId && s.SupplierName.Contains(k)));
        }
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id).Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();

        // 联供应商表，补供应商名称
        var supplierIds = rows.Select(d => d.SupplierId).Distinct().ToList();
        var supMap = await _db.MdSuppliers.Where(s => supplierIds.Contains(s.Id))
                       .ToDictionaryAsync(s => s.Id, s => s.SupplierName);

        var ids = rows.Select(d => d.Id).ToList();
        var lines = await (from d in _db.PoOrderDetails
                           join m in _db.MdMaterials on d.MaterialId equals m.Id
                           where ids.Contains(d.OrderId)
                           select new { d.OrderId, m.MaterialName, d.Quantity }).ToListAsync();
        var matMap = lines.GroupBy(x => x.OrderId)
                          .ToDictionary(g => g.Key, g => string.Join("；", g.Select(x => $"{x.MaterialName}×{x.Quantity.ToString("0.####")}")));

        return new Page_Result
        {
            total = total,
            rows = rows.Select(r => new
            {
                r.Id, r.OrderNo, r.SupplierId, r.OrderDate, r.TotalAmount, r.Status,
                SupplierName = supMap.TryGetValue(r.SupplierId, out var name) ? name : "",
                Materials = matMap.TryGetValue(r.Id, out var ms) ? ms : ""
            }).ToList()
        };
    }

    // ==================================================================
    //  采购订单变更
    // ==================================================================

    /// <summary>
    /// 采购订单变更：调整明细数量/单价，逐项写入变更留痕表。
    /// 已收货数量为下限，数量不得低于已收数量。
    /// </summary>
    public async Task<object> ChangeAsync(Order_Change_Request req)
    {
        var order = await _db.PoOrders.FindAsync(req.orderId) ?? throw new Biz_Exception("采购订单不存在");
        if (order.Status >= Order_State_Machine.Po.Completed)
            throw new Biz_Exception("订单已完成，不允许变更");

        var details = await _db.PoOrderDetails.Where(d => d.OrderId == order.Id).ToListAsync();
        var changes = new List<SysBillChange>();

        foreach (var line in req.lines)
        {
            var dtl = details.FirstOrDefault(d => d.MaterialId == line.materialId);
            if (dtl == null)
            {
                if ((line.quantity ?? 0) <= 0) continue;
                _db.PoOrderDetails.Add(new PoOrderDetail
                {
                    OrderId = order.Id,
                    MaterialId = line.materialId,
                    Quantity = line.quantity!.Value,
                    UnitPrice = line.unitPrice ?? 0,
                    ReceivedQty = 0
                });
                changes.Add(NewChange("PO", order.Id, order.OrderNo, "ADD", null,
                    $"物料{line.materialId} 数量{line.quantity} 单价{line.unitPrice}", req.reason));
                continue;
            }

            if (line.quantity.HasValue && line.quantity.Value != dtl.Quantity)
            {
                var received = dtl.ReceivedQty ?? 0;
                if (line.quantity.Value < received)
                    throw new Biz_Exception($"物料 {line.materialId} 已收 {received}，变更后数量不能小于已收数量");
                changes.Add(NewChange("PO", order.Id, order.OrderNo, "QTY",
                    dtl.Quantity.ToString(), line.quantity.Value.ToString(), req.reason));
                dtl.Quantity = line.quantity.Value;
            }
            if (line.unitPrice.HasValue && line.unitPrice.Value != dtl.UnitPrice)
            {
                changes.Add(NewChange("PO", order.Id, order.OrderNo, "PRICE",
                    dtl.UnitPrice.ToString(), line.unitPrice.Value.ToString(), req.reason));
                dtl.UnitPrice = line.unitPrice.Value;
            }
        }

        var allDetails = await _db.PoOrderDetails.Where(d => d.OrderId == order.Id).ToListAsync();
        order.TotalAmount = allDetails.Sum(d => d.Quantity * d.UnitPrice);

        if (changes.Count > 0) _db.SysBillChanges.AddRange(changes);
        await _db.SaveChangesAsync();
        return new { order, changeCount = changes.Count };
    }

    private static SysBillChange NewChange(string billType, long billId, string? billNo,
        string changeType, string? before, string? after, string? reason)
        => new()
        {
            BillType = billType,
            BillId = billId,
            BillNo = billNo,
            ChangeType = changeType,
            BeforeValue = before,
            AfterValue = after,
            Reason = reason,
            OperTime = DateTime.Now
        };

    // ==================================================================
    //  请购单
    // ==================================================================

    /// <summary>
    /// 新建请购单。明细自动按供应商报价推荐最优供应商与建议单价。
    /// </summary>
    public async Task<object> CreateRequisitionAsync(Requisition_Create_Request req)
    {
        if (req.lines.Count == 0) throw new Biz_Exception("请购明细不能为空");

        var reqBill = new PoRequisition
        {
            ReqNo = await _no.NextAsync("REQ"),
            ReqDate = req.reqDate,
            DeptName = req.deptName,
            Status = 0,
            Remark = req.remark,
            CreateTime = DateTime.Now
        };
        _db.PoRequisitions.Add(reqBill);
        await _db.SaveChangesAsync();

        foreach (var line in req.lines)
        {
            var best = await BestPriceAsync(line.materialId);
            _db.PoRequisitionDetails.Add(new PoRequisitionDetail
            {
                ReqId = reqBill.Id,
                MaterialId = line.materialId,
                ReqQty = line.reqQty,
                ConvertedQty = 0,
                SuggestSupplierId = best?.SupplierId,
                SuggestPrice = best?.Price,
                Remark = line.remark
            });
        }
        await _db.SaveChangesAsync();
        return reqBill;
    }

    public async Task<object> RequisitionPageAsync(Page_Request req)
    {
        var q = _db.PoRequisitions.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => d.ReqNo.Contains(req.keyword) || (d.DeptName ?? "").Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    /// <summary>请购转采购订单：按明细生成采购订单，并回写请购单状态。</summary>
    public async Task<object> RequisitionToPoAsync(Requisition_To_Po_Request req)
    {
        var reqBill = await _db.PoRequisitions.FindAsync(req.reqId) ?? throw new Biz_Exception("请购单不存在");
        if (reqBill.Status == 1) throw new Biz_Exception("该请购单已转为采购订单");
        if (req.supplierId <= 0) throw new Biz_Exception("请选择供应商");

        var details = await _db.PoRequisitionDetails.Where(d => d.ReqId == req.reqId).ToListAsync();
        if (details.Count == 0) throw new Biz_Exception("请购单没有明细");

        var lines = new List<Order_Line_Dto>();
        foreach (var d in details)
        {
            // 优先取报价价，没有报价则用最近一次采购成交价
            var price = d.SuggestPrice ?? await LastPurchasePriceAsync(d.MaterialId);
            lines.Add(new Order_Line_Dto
            {
                materialId = d.MaterialId,
                quantity = d.ReqQty,
                unitPrice = price
            });
        }

        var order = await CreateAsync(new Po_Create_Request
        {
            supplierId = req.supplierId,
            orderDate = req.orderDate ?? reqBill.ReqDate,
            lines = lines
        });

        reqBill.Status = 1;
        var idProp = order.GetType().GetProperty("Id");
        reqBill.PoOrderId = idProp == null ? null : Convert.ToInt64(idProp.GetValue(order));

        foreach (var d in details) d.ConvertedQty = d.ReqQty;
        await _db.SaveChangesAsync();
        return order;
    }

    /// <summary>该物料最近一次采购成交单价。</summary>
    private async Task<decimal> LastPurchasePriceAsync(long materialId)
    {
        var last = await _db.PoOrderDetails
            .Where(d => d.MaterialId == materialId)
            .OrderByDescending(d => d.Id)
            .Select(d => d.UnitPrice)
            .FirstOrDefaultAsync();
        return last;
    }

    // ==================================================================
    //  供应商报价与绩效评估
    // ==================================================================

    /// <summary>登记 / 更新供应商报价（同一供应商+物料只保留一条有效报价）。</summary>
    public async Task<object> SaveQuoteAsync(Supplier_Quote_Request req)
    {
        if (req.quotePrice <= 0) throw new Biz_Exception("报价单价必须大于0");
        var quote = await _db.PoSupplierQuotes.FirstOrDefaultAsync(d =>
            d.SupplierId == req.supplierId && d.MaterialId == req.materialId);
        if (quote == null)
        {
            quote = new PoSupplierQuote
            {
                SupplierId = req.supplierId,
                MaterialId = req.materialId,
                QuotePrice = req.quotePrice,
                ValidFrom = req.validFrom,
                ValidTo = req.validTo,
                CreateTime = DateTime.Now
            };
            _db.PoSupplierQuotes.Add(quote);
        }
        else
        {
            quote.QuotePrice = req.quotePrice;
            quote.ValidFrom = req.validFrom;
            quote.ValidTo = req.validTo;
        }
        await _db.SaveChangesAsync();
        return quote;
    }

    public async Task<object> QuotePageAsync(Page_Request req, long? materialId)
    {
        // 联供应商/物料表，返回名称供列表展示
        var q = from d in _db.PoSupplierQuotes
                join s in _db.MdSuppliers on d.SupplierId equals s.Id
                join m in _db.MdMaterials on d.MaterialId equals m.Id
                select new
                {
                    d.Id, d.SupplierId, s.SupplierName,
                    d.MaterialId, m.MaterialCode, m.MaterialName,
                    d.QuotePrice, d.ValidFrom, d.ValidTo
                };
        if (materialId.HasValue) q = q.Where(d => d.MaterialId == materialId.Value);
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => d.SupplierName.Contains(req.keyword) || d.MaterialCode.Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    /// <summary>
    /// 最优报价：在有效期内（valid_from ≤ 今天 ≤ valid_to 或未设边界）
    /// 的报价中取最低价，无有效报价时回落到最近一次采购成交价。
    /// </summary>
    public async Task<Best_Price_Result?> BestPriceAsync(long materialId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var quotes = await _db.PoSupplierQuotes
            .Where(d => d.MaterialId == materialId
                     && (d.ValidFrom == null || d.ValidFrom <= today)
                     && (d.ValidTo == null || d.ValidTo >= today))
            .ToListAsync();

        var best = quotes.OrderBy(d => d.QuotePrice).FirstOrDefault();
        if (best == null) return null;

        var supplier = await _db.MdSuppliers.FindAsync(best.SupplierId);
        return new Best_Price_Result
        {
            SupplierId = best.SupplierId,
            SupplierName = supplier?.SupplierName,
            Price = best.QuotePrice,
            ValidFrom = best.ValidFrom,
            ValidTo = best.ValidTo
        };
    }

    /// <summary>
    /// 供应商绩效评估：
    ///   订单准时率 —— 按交期完成收货的订单占比；
    ///   合格率     —— 质检通过数量 / 收货总数量；
    ///   贡献度     —— 累计采购金额占比。
    /// </summary>
    public async Task<object> PerformanceAsync(long supplierId)
    {
        var supplier = await _db.MdSuppliers.FindAsync(supplierId) ?? throw new Biz_Exception("供应商不存在");

        var orders = await _db.PoOrders.Where(d => d.SupplierId == supplierId).ToListAsync();
        var orderIds = orders.Select(o => o.Id).ToList();

        var receipts = await _db.PoReceipts.Where(d => orderIds.Contains(d.OrderId)).ToListAsync();
        var receiptIds = receipts.Select(r => r.Id).ToList();
        var receiptDetails = await _db.PoReceiptDetails.Where(d => receiptIds.Contains(d.ReceiptId)).ToListAsync();

        var totalReceived = receiptDetails.Sum(d => d.Quantity);
        var totalQualified = receiptDetails.Sum(d => d.QualifiedQty ?? 0m);
        var passRate = totalReceived > 0 ? Math.Round((double)(totalQualified / totalReceived * 100), 2) : 0d;

        // 准时率：收货日期不晚于订单日期 + 30 天视为准时
        var onTime = 0;
        foreach (var r in receipts)
        {
            var o = orders.FirstOrDefault(x => x.Id == r.OrderId);
            if (o == null) continue;
            var deadline = o.OrderDate.AddDays(30);
            if (r.ReceiptDate <= deadline) onTime++;
        }
        var onTimeRate = receipts.Count > 0 ? Math.Round((double)(onTime / receipts.Count * 100), 2) : 0d;

        var totalAmount = orders.Sum(o => o.TotalAmount ?? 0);
        var allAmount = await _db.PoOrders.SumAsync(o => o.TotalAmount ?? 0);

        // 综合评分：准时率 40% + 合格率 40% + 订单完成率 20%
        var completed = orders.Count(o => o.Status == Order_State_Machine.Po.Completed);
        var completeRate = orders.Count > 0 ? Math.Round((double)(completed / orders.Count * 100), 2) : 0d;
        var score = Math.Round(onTimeRate * 0.4 + passRate * 0.4 + completeRate * 0.2, 2);

        return new
        {
            supplierId,
            supplierCode = supplier.SupplierCode,
            supplierName = supplier.SupplierName,
            orderCount = orders.Count,
            receiptCount = receipts.Count,
            completedCount = completed,
            totalAmount,
            contribution = allAmount > 0 ? Math.Round((double)(totalAmount / allAmount * 100), 2) : 0d,
            totalReceived,
            totalQualified,
            passRate,
            onTimeRate,
            completeRate,
            score,
            grade = score >= 90 ? "A" : score >= 80 ? "B" : score >= 60 ? "C" : "D"
        };
    }
}

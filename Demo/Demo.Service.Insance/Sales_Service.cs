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
public class Sales_Service : I_Sales_Service
{
    private readonly ErpDbContext _db;
    private readonly BillNo_Helper _no;
    private readonly I_Inventory_Service _inv;

    public Sales_Service(ErpDbContext db, BillNo_Helper no, I_Inventory_Service inv)
    {
        _db = db;
        _no = no;
        _inv = inv;
    }

    public async Task<object> CreateAsync(So_Create_Request req)
    {
        if (req.lines.Count == 0) throw new Biz_Exception("订单明细不能为空");
        var amount = req.lines.Sum(x => x.quantity * x.unitPrice);
        await CheckCreditAsync(req.customerId, amount);

        await using var tx = await _db.Database.BeginTransactionAsync();
        var order = new SoOrder
        {
            OrderNo = await _no.NextAsync("SO"),
            CustomerId = req.customerId,
            OrderDate = req.orderDate,
            DeliveryDate = req.deliveryDate,
            Status = Order_State_Machine.So.New,
            TotalAmount = amount,
            CreatedAt = DateTime.Now
        };
        _db.SoOrders.Add(order);
        await _db.SaveChangesAsync();
        foreach (var line in req.lines)
        {
            _db.SoOrderDetails.Add(new SoOrderDetail
            {
                OrderId = order.Id,
                MaterialId = line.materialId,
                Quantity = line.quantity,
                UnitPrice = line.unitPrice,
                ShippedQty = 0
            });
        }
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return order;
    }

    public async Task<object> ConfirmAsync(long id)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        var order = await _db.SoOrders.FindAsync(id) ?? throw new Biz_Exception("销售订单不存在");
        order.Status = Order_State_Machine.So.Transit(order.Status, Order_State_Machine.So.Confirmed);
        var credit = (await _db.SoCustomerCredits
            .FromSqlInterpolated($"SELECT * FROM so_customer_credit WHERE customer_id = {order.CustomerId} LIMIT 1 FOR UPDATE")
            .ToListAsync()).FirstOrDefault();
        if (credit != null)
        {
            credit.UsedAmount = (credit.UsedAmount ?? 0) + (order.TotalAmount ?? 0);
            credit.UpdateTime = DateTime.Now;
        }
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return order;
    }

    public async Task<object> DeliverAsync(Delivery_Create_Request req)
    {
        var order = await _db.SoOrders.FindAsync(req.orderId) ?? throw new Biz_Exception("销售订单不存在");
        if (order.Status is not (1 or 2)) throw new Biz_Exception("仅已确认/部分发货订单可发货");

        await using var tx = await _db.Database.BeginTransactionAsync();
        var delivery = new SoDelivery
        {
            DeliveryNo = await _no.NextAsync("DN"),
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            DeliveryDate = req.deliveryDate,
            Status = 1,
            CreateTime = DateTime.Now
        };
        _db.SoDeliveries.Add(delivery);
        await _db.SaveChangesAsync();

        var details = await _db.SoOrderDetails.Where(d => d.OrderId == order.Id).ToListAsync();
        foreach (var line in req.lines)
        {
            var dtl = details.FirstOrDefault(x => x.MaterialId == line.materialId) ?? throw new Biz_Exception("发货物料不在订单中");
            if ((dtl.ShippedQty ?? 0) + line.quantity > dtl.Quantity)
                throw new Biz_Exception("超订单数量发货");
            dtl.ShippedQty = (dtl.ShippedQty ?? 0) + line.quantity;
            _db.SoDeliveryDetails.Add(new SoDeliveryDetail { DeliveryId = delivery.Id, MaterialId = line.materialId, Quantity = line.quantity });
            await _inv.ApplyQtyAsync(line.materialId, req.warehouseId, line.quantity, -1, "OUT_SO", delivery.DeliveryNo);
        }

        var allShipped = details.All(x => (x.ShippedQty ?? 0) >= x.Quantity);
        if (allShipped && order.Status == Order_State_Machine.So.Confirmed)
            order.Status = Order_State_Machine.So.Transit(order.Status, Order_State_Machine.So.Shipped);

        _db.FinReceivables.Add(new FinReceivable
        {
            RecvNo = await _no.NextAsync("AR"),
            CustomerId = order.CustomerId,
            SourceOrderId = order.Id,
            Amount = req.lines.Sum(x =>
            {
                var d = details.First(t => t.MaterialId == x.materialId);
                return x.quantity * d.UnitPrice;
            }),
            ReceivedAmount = 0,
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
            Status = 0,
            CreateTime = DateTime.Now
        });

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return delivery;
    }

    public async Task<object> CompleteAsync(long id)
    {
        var order = await _db.SoOrders.FindAsync(id) ?? throw new Biz_Exception("销售订单不存在");
        order.Status = Order_State_Machine.So.Transit(order.Status, Order_State_Machine.So.Completed);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<object> PageAsync(Page_Request req)
    {
        var q = _db.SoOrders.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.OrderNo.Contains(k)
                || _db.MdCustomers.Any(c => c.Id == d.CustomerId && c.CustomerName.Contains(k)));
        }
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id).Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();

        // 附加物料汇总：物料A×2；物料B×1（列表页直接展示，免开详情）
        var ids = rows.Select(d => d.Id).ToList();
        var lines = await (from d in _db.SoOrderDetails
                           join m in _db.MdMaterials on d.MaterialId equals m.Id
                           where ids.Contains(d.OrderId)
                           select new { d.OrderId, m.MaterialName, d.Quantity }).ToListAsync();
        var map = lines.GroupBy(x => x.OrderId)
                       .ToDictionary(g => g.Key, g => string.Join("；", g.Select(x => $"{x.MaterialName}×{x.Quantity.ToString("0.####")}")));

        return new Page_Result
        {
            total = total,
            rows = rows.Select(r => new
            {
                r.Id, r.OrderNo, r.CustomerId, r.OrderDate, r.DeliveryDate,
                r.TotalAmount, r.Status, r.CreatedAt,
                Materials = map.TryGetValue(r.Id, out var s) ? s : ""
            }).ToList()
        };
    }

    private async Task CheckCreditAsync(long customerId, decimal amount)
    {
        var credit = await _db.SoCustomerCredits.FirstOrDefaultAsync(d => d.CustomerId == customerId);
        if (credit == null) return;
        if (credit.Status == 0) throw new Biz_Exception("客户信用已冻结");
        var used = credit.UsedAmount ?? 0;
        var limit = credit.CreditLimit ?? 0;
        if (limit > 0 && used + amount > limit)
            throw new Biz_Exception($"超信用额度，额度 {limit}，已用 {used}，本单 {amount}");
    }

    // ==================================================================
    //  销售报价与价格策略
    // ==================================================================

    /// <summary>
    /// 新建销售报价单。
    /// 价格策略决定明细单价取值：
    ///   STANDARD 标准价 —— 取该物料最近一次销售成交单价；
    ///   PREFER   优选价 —— 取该客户历史成交单价中的最低值；
    ///   VIP      会员价 —— 标准价 × 折扣率；
    ///   CUSTOM   协议价 —— 直接采用请求传入的单价。
    /// 明细单价为 0 时按策略自动取价，避免人工录入错误。
    /// </summary>
    public async Task<object> CreateQuoteAsync(Quote_Create_Request req)
    {
        if (req.lines.Count == 0) throw new Biz_Exception("报价明细不能为空");

        var quote = new SoQuote
        {
            QuoteNo = await _no.NextAsync("QT"),
            CustomerId = req.customerId,
            QuoteDate = req.quoteDate,
            ValidDate = req.validDate,
            PriceStrategy = req.priceStrategy,
            DiscountRate = req.discountRate ?? 1m,
            Status = 0,
            Remark = req.remark,
            CreateTime = DateTime.Now
        };

        decimal total = 0;
        foreach (var line in req.lines)
        {
            var unitPrice = line.unitPrice;
            if (unitPrice <= 0)
                unitPrice = await ResolvePriceAsync(req.priceStrategy, req.customerId, line.materialId);

            var lineRate = line.discountRate ?? 1m;
            var amount = Math.Round(line.quantity * unitPrice * lineRate, 2);
            total += amount;

            _db.SoQuoteDetails.Add(new SoQuoteDetail
            {
                QuoteId = quote.Id,
                MaterialId = line.materialId,
                Quantity = line.quantity,
                UnitPrice = unitPrice,
                DiscountRate = lineRate,
                Amount = amount
            });
        }

        // 表头折扣作用在总额上
        quote.TotalAmount = Math.Round(total * (quote.DiscountRate ?? 1m), 2);
        _db.SoQuotes.Add(quote);
        await _db.SaveChangesAsync();
        return quote;
    }

    public async Task<object> QuotePageAsync(Page_Request req)
    {
        var q = _db.SoQuotes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.QuoteNo.Contains(k) || (d.Remark ?? "").Contains(k)
                || _db.MdCustomers.Any(c => c.Id == d.CustomerId && c.CustomerName.Contains(k)));
        }
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    public async Task<object> QuoteDetailAsync(long id)
    {
        var quote = await _db.SoQuotes.FindAsync(id) ?? throw new Biz_Exception("报价单不存在");
        var details = await _db.SoQuoteDetails.Where(d => d.QuoteId == id).ToListAsync();
        return new { quote, details };
    }

    /// <summary>报价生效：草稿 → 已生效。</summary>
    public async Task<object> ConfirmQuoteAsync(long id)
    {
        var quote = await _db.SoQuotes.FindAsync(id) ?? throw new Biz_Exception("报价单不存在");
        if (quote.Status != 0) throw new Biz_Exception("仅草稿状态的报价单可生效");
        quote.Status = 1;
        await _db.SaveChangesAsync();
        return quote;
    }

    /// <summary>报价转销售订单：按报价明细生成销售订单并占用客户信用额度。</summary>
    public async Task<object> QuoteToOrderAsync(Quote_To_Order_Request req)
    {
        var quote = await _db.SoQuotes.FindAsync(req.quoteId) ?? throw new Biz_Exception("报价单不存在");
        if (quote.Status != 1) throw new Biz_Exception("仅已生效的报价单可转订单");

        var details = await _db.SoQuoteDetails.Where(d => d.QuoteId == req.quoteId).ToListAsync();
        if (details.Count == 0) throw new Biz_Exception("报价单没有明细");

        var lines = details.Select(d => new Order_Line_Dto
        {
            materialId = d.MaterialId,
            quantity = d.Quantity,
            unitPrice = d.UnitPrice * (d.DiscountRate ?? 1m)
        }).ToList();

        var order = await CreateAsync(new So_Create_Request
        {
            customerId = quote.CustomerId,
            orderDate = quote.QuoteDate,
            deliveryDate = req.deliveryDate,
            lines = lines
        });

        quote.Status = 2;
        var orderId = ExtractId(order);
        quote.OrderId = orderId;
        await _db.SaveChangesAsync();
        return order;
    }

    /// <summary>按价格策略解析物料单价。</summary>
    private async Task<decimal> ResolvePriceAsync(string strategy, long customerId, long materialId)
    {
        switch (strategy?.ToUpperInvariant())
        {
            case "PREFER":
                {
                    // 该客户历史成交单价中的最低值
                    var min = await (from d in _db.SoOrderDetails
                                     join o in _db.SoOrders on d.OrderId equals o.Id
                                     where o.CustomerId == customerId && d.MaterialId == materialId
                                     select d.UnitPrice).MinAsync();
                    if (min > 0) return min;
                    break;
                }
            case "VIP":
                {
                    // 标准价 × 0.9（会员九折）
                    var std = await ResolveStandardPriceAsync(materialId);
                    return Math.Round(std * 0.9m, 6);
                }
        }

        // STANDARD / CUSTOM 及兜底：标准价
        return await ResolveStandardPriceAsync(materialId);
    }

    /// <summary>标准价：该物料最近一次销售成交单价，无成交记录取 0。</summary>
    private async Task<decimal> ResolveStandardPriceAsync(long materialId)
    {
        var last = await (from d in _db.SoOrderDetails
                          join o in _db.SoOrders on d.OrderId equals o.Id
                          where d.MaterialId == materialId
                          orderby o.Id descending
                          select d.UnitPrice).FirstOrDefaultAsync();
        return last;
    }

    private static long? ExtractId(object entity)
    {
        if (entity == null) return null;
        var prop = entity.GetType().GetProperty("Id");
        if (prop == null) return null;
        var val = prop.GetValue(entity);
        return val == null ? null : Convert.ToInt64(val);
    }

    // ==================================================================
    //  销售订单变更
    // ==================================================================

    /// <summary>
    /// 销售订单变更：调整明细数量/价格与交期，逐项写入变更留痕表。
    /// 已发货数量为下限，数量不得低于已发数量。
    /// </summary>
    public async Task<object> ChangeAsync(Order_Change_Request req)
    {
        var order = await _db.SoOrders.FindAsync(req.orderId) ?? throw new Biz_Exception("销售订单不存在");
        if (order.Status >= Order_State_Machine.So.Shipped)
            throw new Biz_Exception("订单已发货，不允许变更");

        var details = await _db.SoOrderDetails.Where(d => d.OrderId == order.Id).ToListAsync();
        var changes = new List<SysBillChange>();

        foreach (var line in req.lines)
        {
            var dtl = details.FirstOrDefault(d => d.MaterialId == line.materialId);
            if (dtl == null)
            {
                // 新增行
                if ((line.quantity ?? 0) <= 0) continue;
                _db.SoOrderDetails.Add(new SoOrderDetail
                {
                    OrderId = order.Id,
                    MaterialId = line.materialId,
                    Quantity = line.quantity!.Value,
                    UnitPrice = line.unitPrice ?? 0,
                    ShippedQty = 0
                });
                changes.Add(NewChange("SO", order.Id, order.OrderNo, "ADD", null,
                    $"物料{line.materialId} 数量{line.quantity} 单价{line.unitPrice}", req.reason));
                continue;
            }

            if (line.quantity.HasValue && line.quantity.Value != dtl.Quantity)
            {
                var shipped = dtl.ShippedQty ?? 0;
                if (line.quantity.Value < shipped)
                    throw new Biz_Exception($"物料 {line.materialId} 已发 {shipped}，变更后数量不能小于已发数量");
                changes.Add(NewChange("SO", order.Id, order.OrderNo, "QTY",
                    dtl.Quantity.ToString(), line.quantity.Value.ToString(), req.reason));
                dtl.Quantity = line.quantity.Value;
            }
            if (line.unitPrice.HasValue && line.unitPrice.Value != dtl.UnitPrice)
            {
                changes.Add(NewChange("SO", order.Id, order.OrderNo, "PRICE",
                    dtl.UnitPrice.ToString(), line.unitPrice.Value.ToString(), req.reason));
                dtl.UnitPrice = line.unitPrice.Value;
            }
        }

        // 交期变更
        if (req.deliveryDate.HasValue && req.deliveryDate.Value != order.DeliveryDate)
        {
            changes.Add(NewChange("SO", order.Id, order.OrderNo, "DATE",
                order.DeliveryDate?.ToString("yyyy-MM-dd"), req.deliveryDate.Value.ToString("yyyy-MM-dd"), req.reason));
            order.DeliveryDate = req.deliveryDate;
        }

        // 重算订单总额
        var allDetails = await _db.SoOrderDetails.Where(d => d.OrderId == order.Id).ToListAsync();
        order.TotalAmount = allDetails.Sum(d => d.Quantity * d.UnitPrice);

        if (changes.Count > 0) _db.SysBillChanges.AddRange(changes);
        await _db.SaveChangesAsync();
        return new { order, changeCount = changes.Count };
    }

    public async Task<object> ChangeLogAsync(string billType, long billId)
    {
        return await _db.SysBillChanges
            .Where(d => d.BillType == billType && d.BillId == billId)
            .OrderByDescending(d => d.Id).ToListAsync();
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
    //  发货与物流跟踪
    // ==================================================================

    public async Task<object> DeliveryPageAsync(Page_Request req)
    {
        var q = _db.SoDeliveries.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.DeliveryNo.Contains(k)
                || _db.MdCustomers.Any(c => c.Id == d.CustomerId && c.CustomerName.Contains(k)));
        }
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    /// <summary>
    /// 登记物流跟踪节点。签收时同步把发货单状态推进到「已签收」。
    /// </summary>
    public async Task<object> AddTrackAsync(Track_Create_Request req)
    {
        var delivery = await _db.SoDeliveries.FindAsync(req.deliveryId) ?? throw new Biz_Exception("发货单不存在");
        var track = new SoDeliveryTrack
        {
            DeliveryId = req.deliveryId,
            Carrier = req.carrier,
            TrackNo = req.trackNo,
            TrackStatus = req.trackStatus,
            Location = req.location,
            Remark = req.remark,
            TrackTime = DateTime.Now
        };
        _db.SoDeliveryTracks.Add(track);

        if (req.trackStatus == "已签收")
            delivery.Status = 2;
        else if (delivery.Status == 0)
            delivery.Status = 1;

        await _db.SaveChangesAsync();
        return track;
    }

    public async Task<object> TrackListAsync(long deliveryId)
    {
        return await _db.SoDeliveryTracks
            .Where(d => d.DeliveryId == deliveryId)
            .OrderBy(d => d.TrackTime).ToListAsync();
    }

    // ==================================================================
    //  客户信用管理
    // ==================================================================

    public async Task<object> CreditPageAsync(Page_Request req)
    {
        var q = from c in _db.SoCustomerCredits
                join cu in _db.MdCustomers on c.CustomerId equals cu.Id
                select new
                {
                    c.Id, c.CustomerId, cu.CustomerCode, cu.CustomerName,
                    c.CreditLimit, c.UsedAmount,
                    Available = (c.CreditLimit ?? 0) - (c.UsedAmount ?? 0),
                    c.Status, c.UpdateTime
                };
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => d.CustomerName.Contains(req.keyword) || d.CustomerCode.Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderBy(d => d.CustomerCode)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    /// <summary>维护客户信用额度。客户没有信用档案时自动建档。</summary>
    public async Task<object> SaveCreditAsync(SoCustomerCredit credit)
    {
        if (credit.CustomerId <= 0) throw new Biz_Exception("请选择客户");
        var row = await _db.SoCustomerCredits.FirstOrDefaultAsync(d => d.CustomerId == credit.CustomerId);
        if (row == null)
        {
            row = new SoCustomerCredit
            {
                CustomerId = credit.CustomerId,
                CreditLimit = credit.CreditLimit ?? 0,
                UsedAmount = 0,
                Status = credit.Status ?? 1,
                UpdateTime = DateTime.Now
            };
            _db.SoCustomerCredits.Add(row);
        }
        else
        {
            row.CreditLimit = credit.CreditLimit ?? 0;
            if (credit.Status.HasValue) row.Status = credit.Status;
            row.UpdateTime = DateTime.Now;
        }
        await _db.SaveChangesAsync();
        return row;
    }

    /// <summary>冻结 / 解冻客户信用。冻结后该客户的新订单会被拦截。</summary>
    public async Task<object> FreezeCreditAsync(long customerId, bool freeze)
    {
        var row = await _db.SoCustomerCredits.FirstOrDefaultAsync(d => d.CustomerId == customerId)
                  ?? throw new Biz_Exception("该客户尚未设置信用档案");
        row.Status = freeze ? (sbyte)0 : (sbyte)1;
        row.UpdateTime = DateTime.Now;
        await _db.SaveChangesAsync();
        return row;
    }
}

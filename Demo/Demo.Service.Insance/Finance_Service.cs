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
public class Finance_Service : I_Finance_Service
{
    private readonly ErpDbContext _db;
    private readonly BillNo_Helper _no;

    public Finance_Service(ErpDbContext db, BillNo_Helper no)
    {
        _db = db;
        _no = no;
    }

    public async Task<object> CreateVoucherAsync(Voucher_Create_Request req)
    {
        if (req.entries.Count == 0) throw new Biz_Exception("分录不能为空");
        var debit = req.entries.Where(x => x.direction == "DR").Sum(x => x.amount);
        var credit = req.entries.Where(x => x.direction == "CR").Sum(x => x.amount);
        if (debit != credit) throw new Biz_Exception("借贷不平衡");

        var v = new FinVoucher
        {
            VoucherNo = await _no.NextAsync("V"),
            VoucherDate = req.voucherDate,
            SourceModule = req.sourceModule,
            SourceId = req.sourceId,
            TotalDebit = debit,
            TotalCredit = credit,
            Status = Order_State_Machine.Voucher.Draft,
            CreateTime = DateTime.Now
        };
        _db.FinVouchers.Add(v);
        await _db.SaveChangesAsync();
        var n = 1;
        foreach (var e in req.entries)
        {
            _db.FinVoucherEntries.Add(new FinVoucherEntry
            {
                VoucherId = v.Id,
                EntryNo = n++,
                AccountCode = e.accountCode,
                Direction = e.direction,
                Amount = e.amount,
                Memo = e.memo
            });
        }
        await _db.SaveChangesAsync();
        return v;
    }

    public async Task<object> AuditVoucherAsync(long id)
    {
        var v = await _db.FinVouchers.FindAsync(id) ?? throw new Biz_Exception("凭证不存在");
        v.Status = Order_State_Machine.Voucher.Transit(v.Status, Order_State_Machine.Voucher.Audited);
        await _db.SaveChangesAsync();
        return v;
    }

    public async Task<object> PostVoucherAsync(long id)
    {
        var v = await _db.FinVouchers.FindAsync(id) ?? throw new Biz_Exception("凭证不存在");
        v.Status = Order_State_Machine.Voucher.Transit(v.Status, Order_State_Machine.Voucher.Posted);
        await _db.SaveChangesAsync();
        return v;
    }

    public async Task<object> CloseVoucherAsync(long id)
    {
        var v = await _db.FinVouchers.FindAsync(id) ?? throw new Biz_Exception("凭证不存在");
        v.Status = Order_State_Machine.Voucher.Transit(v.Status, Order_State_Machine.Voucher.Closed);
        await _db.SaveChangesAsync();
        return v;
    }

    public async Task<object> WriteoffReceiptAsync(Receipt_Writeoff_Request req)
    {
        var ar = await _db.FinReceivables.FindAsync(req.receivableId) ?? throw new Biz_Exception("应收单不存在");
        if (ar.CustomerId != req.customerId) throw new Biz_Exception("客户不匹配");
        var left = ar.Amount - (ar.ReceivedAmount ?? 0);
        if (req.amount <= 0 || req.amount > left) throw new Biz_Exception("核销金额非法");

        ar.ReceivedAmount = (ar.ReceivedAmount ?? 0) + req.amount;
        ar.Status = (sbyte)(ar.ReceivedAmount >= ar.Amount ? 2 : 1);
        _db.FinReceipts.Add(new FinReceipt
        {
            ReceiptNo = await _no.NextAsync("RC"),
            CustomerId = req.customerId,
            ReceivableId = ar.Id,
            Amount = req.amount,
            ReceiptDate = req.receiptDate,
            CreateTime = DateTime.Now
        });
        await _db.SaveChangesAsync();
        return ar;
    }

    public async Task<object> WriteoffPaymentAsync(Payment_Writeoff_Request req)
    {
        var ap = await _db.FinPayables.FindAsync(req.payableId) ?? throw new Biz_Exception("应付单不存在");
        if (ap.SupplierId != req.supplierId) throw new Biz_Exception("供应商不匹配");
        var left = ap.Amount - (ap.PaidAmount ?? 0);
        if (req.amount <= 0 || req.amount > left) throw new Biz_Exception("核销金额非法");

        ap.PaidAmount = (ap.PaidAmount ?? 0) + req.amount;
        ap.Status = (sbyte)(ap.PaidAmount >= ap.Amount ? 2 : 1);
        _db.FinPayments.Add(new FinPayment
        {
            PaymentNo = await _no.NextAsync("PM"),
            SupplierId = req.supplierId,
            PayableId = ap.Id,
            Amount = req.amount,
            PaymentDate = req.paymentDate,
            CreateTime = DateTime.Now
        });
        await _db.SaveChangesAsync();
        return ap;
    }

    public async Task<object> AgeingAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var ar = await _db.FinReceivables.Where(d => d.Status != 2).ToListAsync();
        var custIds = ar.Select(d => d.CustomerId).Distinct().ToList();
        var custMap = await _db.MdCustomers.Where(c => custIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => c.CustomerName);
        var buckets = ar.Select(d =>
        {
            var due = d.DueDate ?? today;
            var days = today.DayNumber - due.DayNumber;
            var open = d.Amount - (d.ReceivedAmount ?? 0);
            return new
            {
                d.RecvNo,
                d.CustomerId,
                CustomerName = custMap.TryGetValue(d.CustomerId, out var cn) ? cn : "",
                open,
                bucket = days <= 0 ? "未到期" : days <= 30 ? "1-30" : days <= 60 ? "31-60" : days <= 90 ? "61-90" : "90+"
            };
        }).ToList();
        return buckets;
    }

    public async Task<object> CalcCostAsync(Cost_Calc_Request req)
    {
        var order = await _db.PrOrders.FindAsync(req.prOrderId) ?? throw new Biz_Exception("生产订单不存在");
        var issues = await _db.PrMaterialIssues.Where(d => d.PrOrderId == order.Id).ToListAsync();
        decimal materialCost = 0;
        foreach (var i in issues)
        {
            var lastPo = await _db.PoOrderDetails.Where(d => d.MaterialId == i.MaterialId).OrderByDescending(d => d.Id).FirstOrDefaultAsync();
            materialCost += i.IssueQty * (lastPo?.UnitPrice ?? 0);
        }
        var qty = order.CompletedQty ?? 0;
        if (qty <= 0) qty = order.PlanQty;
        var total = materialCost + req.laborCost + req.overheadCost;
        var cost = new FinCost
        {
            CostNo = await _no.NextAsync("COST"),
            PrOrderId = order.Id,
            MaterialId = order.MaterialId,
            MaterialCost = materialCost,
            LaborCost = req.laborCost,
            OverheadCost = req.overheadCost,
            TotalCost = total,
            UnitCost = qty == 0 ? 0 : total / qty,
            CalcDate = DateOnly.FromDateTime(DateTime.Today),
            CreateTime = DateTime.Now
        };
        _db.FinCosts.Add(cost);
        await _db.SaveChangesAsync();
        return cost;
    }

    /// <summary>
    /// 凭证列表（总账）：支持按凭证号/来源模块模糊查询
    /// </summary>
    public async Task<object> PageVoucherAsync(Page_Request req)
    {
        var q = _db.FinVouchers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.VoucherNo.Contains(k)
                || (d.SourceModule ?? "").Contains(k)
                || _db.FinVoucherEntries.Any(e => e.VoucherId == d.Id
                    && ((e.AccountCode ?? "").Contains(k) || (e.Memo ?? "").Contains(k))));
        }
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    /// <summary>
    /// 凭证明细：主表 + 分录
    /// </summary>
    public async Task<object> VoucherDetailAsync(long id)
    {
        var v = await _db.FinVouchers.FindAsync(id) ?? throw new Biz_Exception("凭证不存在");
        var entries = await _db.FinVoucherEntries.Where(d => d.VoucherId == id)
            .OrderBy(d => d.EntryNo).ToListAsync();
        return new { voucher = v, entries };
    }

    /// <summary>
    /// 应收单列表：支持按单号/客户查询
    /// </summary>
    public async Task<object> PageReceivableAsync(Page_Request req)
    {
        var q = _db.FinReceivables.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.RecvNo.Contains(k)
                || _db.MdCustomers.Any(c => c.Id == d.CustomerId && c.CustomerName.Contains(k)));
        }
        var total = await q.CountAsync();
        var rows = await (from d in q
                          join c in _db.MdCustomers on d.CustomerId equals c.Id into cg
                          from c in cg.DefaultIfEmpty()
                          orderby d.Id descending
                          select new
                          {
                              d.Id, d.RecvNo, d.CustomerId, d.Amount, d.ReceivedAmount, d.DueDate, d.Status, d.CreateTime,
                              CustomerName = c.CustomerName ?? ""
                          })
                         .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    /// <summary>
    /// 应付单列表：支持按单号/供应商查询
    /// </summary>
    public async Task<object> PagePayableAsync(Page_Request req)
    {
        var q = _db.FinPayables.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
        {
            var k = req.keyword.Trim();
            q = q.Where(d => d.PayNo.Contains(k)
                || _db.MdSuppliers.Any(s => s.Id == d.SupplierId && s.SupplierName.Contains(k)));
        }
        var total = await q.CountAsync();
        var rows = await (from d in q
                          join s in _db.MdSuppliers on d.SupplierId equals s.Id into sg
                          from s in sg.DefaultIfEmpty()
                          orderby d.Id descending
                          select new
                          {
                              d.Id, d.PayNo, d.SupplierId, d.Amount, d.PaidAmount, d.DueDate, d.Status, d.CreateTime,
                              SupplierName = s.SupplierName ?? ""
                          })
                         .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }
}

using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Service.Instance;

[Inject_Attribute]
public class Dashboard_Service : I_Dashboard_Service
{
    private readonly ErpDbContext _db;
    private readonly Redis_Cache _cache;

    public Dashboard_Service(ErpDbContext db, Redis_Cache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<object> OverviewAsync()
    {
        var cached = await _cache.GetAsync<object>("erp:dashboard");
        if (cached != null) return cached;

        var soAmount = await _db.SoOrders.SumAsync(d => d.TotalAmount ?? 0);
        var poAmount = await _db.PoOrders.SumAsync(d => d.TotalAmount ?? 0);
        var arOpen = await _db.FinReceivables.Where(d => d.Status != 2).SumAsync(d => d.Amount - (d.ReceivedAmount ?? 0));
        var apOpen = await _db.FinPayables.Where(d => d.Status != 2).SumAsync(d => d.Amount - (d.PaidAmount ?? 0));
        var stockSku = await _db.InvStocks.CountAsync();
        var alerts = await (from s in _db.InvStocks
                            join m in _db.MdMaterials on s.MaterialId equals m.Id
                            where m.SafetyStock != null && s.CurrentQty < m.SafetyStock
                            select s.Id).CountAsync();
        var prOpen = await _db.PrOrders.CountAsync(d => d.Status != 3 && d.Status != 4);
        var soByStatus = await _db.SoOrders.GroupBy(d => d.Status).Select(g => new { status = g.Key, count = g.Count() }).ToListAsync();

        var data = new
        {
            soAmount,
            poAmount,
            arOpen,
            apOpen,
            stockSku,
            alerts,
            prOpen,
            soByStatus,
            gross = soAmount - poAmount
        };
        await _cache.SetAsync("erp:dashboard", data, TimeSpan.FromMinutes(1));
        return data;
    }
}

using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Service.Instance;

/// <summary>
/// 决策报表服务实现。
/// ------------------------------------------------------------------
/// 报表取数原则：
///  1. 成本价统一取「该物料最近一次采购单价」，无采购记录的物料按 0 计；
///  2. 库存周转以「出库数量」作为销量口径，窗口由 days 参数控制；
///  3. 所有聚合均在数据库侧完成（FromSqlRaw），避免把全量明细拉回内存。
/// </summary>
[Inject_Attribute]
public class Rpt_Service : I_Rpt_Service
{
    private readonly ErpDbContext _db;
    private readonly Redis_Cache _cache;

    public Rpt_Service(ErpDbContext db, Redis_Cache cache)
    {
        _db = db;
        _cache = cache;
    }

    /// <summary>
    /// 库存周转分析。
    /// 周转率 = 期间出库数量 / 平均库存数量；周转天数 = 期间天数 / 周转率。
    /// 呆滞料：期间无出库但仍有库存的物料。
    /// </summary>
    public async Task<object> TurnoverAsync(int days)
    {
        if (days <= 0) days = 90;
        var cacheKey = $"erp:rpt:turnover:{days}";
        var cached = await _cache.GetAsync<object>(cacheKey);
        if (cached != null) return cached;

        var since = DateTime.Now.AddDays(-days);

        var rows = await _db.Database.SqlQuery<Rpt_Turnover_Row>($"""
            SELECT
                m.id                       AS MaterialId,
                m.material_code            AS MaterialCode,
                m.material_name            AS MaterialName,
                m.category                 AS Category,
                IFNULL(st.cur_qty, 0)     AS CurrentQty,
                IFNULL(st.last_price, 0)   AS LastPrice,
                IFNULL(st.cur_qty, 0) * IFNULL(st.last_price, 0) AS StockValue,
                IFNULL(out_q.out_qty, 0)  AS OutQty,
                IFNULL(out_q.out_value, 0) AS OutValue
            FROM md_material m
            LEFT JOIN (
                SELECT s.material_id,
                       SUM(s.current_qty)          AS cur_qty,
                       MAX(last_price)            AS last_price
                FROM inv_stock s
                LEFT JOIN (
                    SELECT d.material_id, d.unit_price AS last_price
                    FROM po_order_detail d
                    JOIN po_order o ON o.id = d.order_id
                    WHERE d.id = (
                        SELECT MAX(d2.id) FROM po_order_detail d2 WHERE d2.material_id = d.material_id
                    )
                ) lp ON lp.material_id = s.material_id
                GROUP BY s.material_id
            ) st ON st.material_id = m.id
            LEFT JOIN (
                SELECT t.material_id,
                       SUM(t.quantity)                    AS out_qty,
                       SUM(t.quantity * IFNULL(lp2.last_price, 0)) AS out_value
                FROM inv_transaction t
                LEFT JOIN (
                    SELECT d.material_id, d.unit_price AS last_price
                    FROM po_order_detail d
                    WHERE d.id = (
                        SELECT MAX(d2.id) FROM po_order_detail d2 WHERE d2.material_id = d.material_id
                    )
                ) lp2 ON lp2.material_id = t.material_id
                WHERE t.direction = -1 AND t.trans_time >= {since}
                GROUP BY t.material_id
            ) out_q ON out_q.material_id = m.id
            WHERE m.status = 1
            """).ToListAsync();

        var list = rows.Select(r =>
        {
            var avgStock = r.CurrentQty;                 // 简化：以当前结存近似平均库存
            var turnoverRate = avgStock > 0 ? Math.Round((double)(r.OutQty / avgStock), 4) : 0d;
            var turnoverDays = turnoverRate > 0 ? Math.Round(days / turnoverRate, 1) : 0d;
            return new
            {
                r.MaterialId,
                r.MaterialCode,
                r.MaterialName,
                r.Category,
                r.CurrentQty,
                r.LastPrice,
                r.StockValue,
                r.OutQty,
                r.OutValue,
                TurnoverRate = turnoverRate,
                TurnoverDays = turnoverDays,
                // 呆滞：有库存但期间零出库
                IsSlowMoving = r.CurrentQty > 0 && r.OutQty == 0
            };
        }).ToList();

        var result = new
        {
            days,
            rows = list,
            summary = new
            {
                materialCount = list.Count,
                stockValue = list.Sum(x => x.StockValue),
                outValue = list.Sum(x => x.OutValue),
                slowMovingCount = list.Count(x => x.IsSlowMoving),
                avgTurnoverDays = list.Where(x => x.TurnoverDays > 0).Any()
                    ? Math.Round(list.Where(x => x.TurnoverDays > 0).Average(x => x.TurnoverDays), 1)
                    : 0d
            }
        };
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <summary>
    /// 生产进度：工单计划量 vs 良品报工量 vs 完成率，并汇总不良率。
    /// </summary>
    public async Task<object> ProductionProgressAsync()
    {
        var cacheKey = "erp:rpt:production";
        var cached = await _cache.GetAsync<object>(cacheKey);
        if (cached != null) return cached;

        var orders = await (from o in _db.PrOrders
                            join m in _db.MdMaterials on o.MaterialId equals m.Id
                            select new
                            {
                                o.Id,
                                o.OrderNo,
                                o.MaterialId,
                                MaterialCode = m.MaterialCode,
                                MaterialName = m.MaterialName,
                                o.PlanQty,
                                o.CompletedQty,
                                o.Status,
                                o.StartDate,
                                o.EndDate
                            }).ToListAsync();

        var reports = await _db.PrReports.AsNoTracking().ToListAsync();
        var reportGroup = reports.GroupBy(r => r.PrOrderId)
            .ToDictionary(g => g.Key, g => new
            {
                ReportQty = g.Sum(x => x.ReportQty),
                GoodQty = g.Sum(x => x.GoodQty ?? 0m),
                BadQty = g.Sum(x => x.BadQty ?? 0m),
                WorkHours = g.Sum(x => x.WorkHours ?? 0m),
                ProcessCount = g.Select(x => x.ProcessName).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Count()
            });

        var rows = orders.Select(o =>
        {
            reportGroup.TryGetValue(o.Id, out var r);
            var good = r?.GoodQty ?? 0m;
            var plan = o.PlanQty;
            return new
            {
                o.Id,
                o.OrderNo,
                o.MaterialId,
                o.MaterialCode,
                o.MaterialName,
                PlanQty = plan,
                CompletedQty = o.CompletedQty ?? 0,
                ReportQty = r?.ReportQty ?? 0,
                GoodQty = good,
                BadQty = r?.BadQty ?? 0,
                WorkHours = r?.WorkHours ?? 0,
                ProcessCount = r?.ProcessCount ?? 0,
                o.Status,
                o.StartDate,
                o.EndDate,
                // 完成率按良品 / 计划量
                ProgressRate = plan > 0 ? Math.Round((double)(good / plan * 100), 2) : 0d,
                // 不良率按不良 / 报工总量
                DefectRate = (r?.ReportQty ?? 0) > 0
                    ? Math.Round((double)(r!.BadQty / r.ReportQty * 100), 2) : 0d
            };
        }).ToList();

        var result = new
        {
            rows,
            summary = new
            {
                totalOrders = rows.Count,
                producing = rows.Count(x => x.Status == 2),
                done = rows.Count(x => x.Status == 3),
                planQty = rows.Sum(x => x.PlanQty),
                goodQty = rows.Sum(x => x.GoodQty),
                badQty = rows.Sum(x => x.BadQty),
                avgProgress = rows.Any(x => x.PlanQty > 0)
                    ? Math.Round(rows.Where(x => x.PlanQty > 0).Average(x => x.ProgressRate), 2) : 0d,
                avgDefectRate = rows.Any(x => x.ReportQty > 0)
                    ? Math.Round(rows.Where(x => x.ReportQty > 0).Average(x => x.DefectRate), 2) : 0d,
                totalWorkHours = rows.Sum(x => x.WorkHours)
            }
        };
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(2));
        return result;
    }

    /// <summary>
    /// 毛利分析：按物料维度统计收入 / 成本 / 毛利 / 毛利率。
    /// 收入取已发货（状态≥已发货）订单明细金额；成本取对应出库数量 × 最近采购单价。
    /// </summary>
    public async Task<object> GrossMarginAsync(DateOnly? from, DateOnly? to)
    {
        var start = from?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Now.AddMonths(-12);
        var end = to?.ToDateTime(TimeOnly.MaxValue) ?? DateTime.Now.AddDays(1);
        var cacheKey = $"erp:rpt:margin:{start:yyyyMMdd}:{end:yyyyMMdd}";
        var cached = await _cache.GetAsync<object>(cacheKey);
        if (cached != null) return cached;

        var rows = await _db.Database.SqlQuery<Rpt_Margin_Row>($"""
            SELECT
                m.id                AS MaterialId,
                m.material_code     AS MaterialCode,
                m.material_name     AS MaterialName,
                m.category          AS Category,
                IFNULL(rev.qty, 0)     AS SaleQty,
                IFNULL(rev.amount, 0)   AS Revenue,
                IFNULL(cst.qty, 0)      AS CostQty,
                IFNULL(cst.amount, 0)   AS Cost
            FROM md_material m
            LEFT JOIN (
                SELECT d.material_id,
                       SUM(d.shipped_qty)                                  AS qty,
                       SUM(d.shipped_qty * d.unit_price)                   AS amount
                FROM so_order_detail d
                JOIN so_order o ON o.id = d.order_id
                WHERE o.status >= 2 AND o.order_date BETWEEN {start} AND {end}
                GROUP BY d.material_id
            ) rev ON rev.material_id = m.id
            LEFT JOIN (
                SELECT t.material_id,
                       SUM(t.quantity)                            AS qty,
                       SUM(t.quantity * IFNULL(lp.last_price, 0)) AS amount
                FROM inv_transaction t
                LEFT JOIN (
                    SELECT d.material_id, d.unit_price AS last_price
                    FROM po_order_detail d
                    WHERE d.id = (
                        SELECT MAX(d2.id) FROM po_order_detail d2 WHERE d2.material_id = d.material_id
                    )
                ) lp ON lp.material_id = t.material_id
                WHERE t.direction = -1 AND t.trans_type IN ('OUT_SO', 'OUT_PR')
                  AND t.trans_time BETWEEN {start} AND {end}
                GROUP BY t.material_id
            ) cst ON cst.material_id = m.id
            WHERE m.status = 1
            """).ToListAsync();

        var list = rows.Select(r =>
        {
            var gross = r.Revenue - r.Cost;
            return new
            {
                r.MaterialId,
                r.MaterialCode,
                r.MaterialName,
                r.Category,
                r.SaleQty,
                r.Revenue,
                r.CostQty,
                r.Cost,
                Gross = gross,
                GrossRate = r.Revenue > 0 ? Math.Round((double)(gross / r.Revenue * 100), 2) : 0d
            };
        }).Where(x => x.Revenue != 0 || x.Cost != 0).ToList();

        var totalRevenue = list.Sum(x => x.Revenue);
        var totalCost = list.Sum(x => x.Cost);

        var result = new
        {
            from = start.ToString("yyyy-MM-dd"),
            to = end.ToString("yyyy-MM-dd"),
            rows = list.OrderByDescending(x => x.Revenue).ToList(),
            summary = new
            {
                totalRevenue,
                totalCost,
                totalGross = totalRevenue - totalCost,
                grossRate = totalRevenue > 0 ? Math.Round((double)((totalRevenue - totalCost) / totalRevenue * 100), 2) : 0d,
                materialCount = list.Count
            }
        };
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <summary>看板 KPI 列表（来自 rpt_kpi，含 RabbitMQ 消费写入的库存预警）。</summary>
    public async Task<object> KpiListAsync(int limit)
    {
        if (limit <= 0) limit = 50;
        return await _db.RptKpis.AsNoTracking()
            .OrderByDescending(d => d.CalcDate).ThenByDescending(d => d.Id)
            .Take(limit).ToListAsync();
    }

    /// <summary>销售趋势：按月汇总销售额 / 成本 / 毛利。</summary>
    public async Task<object> SalesTrendAsync(int months)
    {
        if (months <= 0) months = 12;
        var start = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-(months - 1));

        var sales = await (from o in _db.SoOrders
                           join d in _db.SoOrderDetails on o.Id equals d.OrderId
                           where o.OrderDate >= start && o.Status >= 2
                           group d by new { o.OrderDate.Year, o.OrderDate.Month } into g
                           select new
                           {
                               Year = g.Key.Year,
                               Month = g.Key.Month,
                               Revenue = g.Sum(x => (x.ShippedQty ?? x.Quantity) * x.UnitPrice)
                           }).ToListAsync();

        var costs = await (from t in _db.InvTransactions
                           where t.TransTime != null
                              && t.TransTime >= start.ToDateTime(TimeOnly.MinValue)
                              && t.Direction == -1 && t.TransType == "OUT_SO"
                           group t by new { t.TransTime!.Value.Year, t.TransTime!.Value.Month } into g
                           select new
                           {
                               Year = g.Key.Year,
                               Month = g.Key.Month,
                               Cost = g.Sum(x => x.Quantity)
                           }).ToListAsync();

        var monthsList = new List<object>();
        for (var i = 0; i < months; i++)
        {
            var d = start.AddMonths(i);
            var s = sales.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month);
            var c = costs.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month);
            var revenue = s?.Revenue ?? 0m;
            var cost = c?.Cost ?? 0m;
            monthsList.Add(new
            {
                Period = d.ToString("yyyy-MM"),
                Year = d.Year,
                Month = d.Month,
                Revenue = revenue,
                Cost = cost,
                Gross = revenue - cost
            });
        }
        return monthsList;
    }
}

/// <summary>库存周转报表的行模型（与 SQL 列别名一一对应）。</summary>
public class Rpt_Turnover_Row
{
    public long MaterialId { get; set; }
    public string? MaterialCode { get; set; }
    public string? MaterialName { get; set; }
    public string? Category { get; set; }
    public decimal CurrentQty { get; set; }
    public decimal LastPrice { get; set; }
    public decimal StockValue { get; set; }
    public decimal OutQty { get; set; }
    public decimal OutValue { get; set; }
}

/// <summary>毛利报表的行模型（与 SQL 列别名一一对应）。</summary>
public class Rpt_Margin_Row
{
    public long MaterialId { get; set; }
    public string? MaterialCode { get; set; }
    public string? MaterialName { get; set; }
    public string? Category { get; set; }
    public decimal SaleQty { get; set; }
    public decimal Revenue { get; set; }
    public decimal CostQty { get; set; }
    public decimal Cost { get; set; }
}

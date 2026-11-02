using System.Reflection;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Service.Instance;

/// <summary>
/// 主数据通用维护：通过泛型 + 反射定位 DbSet，统一分页/保存。
/// </summary>
[Inject_Attribute]
public class Md_Service : I_Md_Service
{
    private readonly ErpDbContext _db;
    private readonly Redis_Cache _cache;

    public Md_Service(ErpDbContext db, Redis_Cache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<object> PageAsync<T>(Page_Request req, Func<IQueryable<T>, IQueryable<T>>? filter = null) where T : class
    {
        IQueryable<T> q = _db.Set<T>();
        if (filter != null) q = filter(q);
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = ApplyKeyword(q, req.keyword);
        var total = await q.CountAsync();
        var rows = await q.Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }

    public async Task<object> SaveAsync<T>(T entity) where T : class
    {
        var idProp = typeof(T).GetProperty("Id");
        var idVal = idProp?.GetValue(entity);
        var isNew = idVal == null || idVal.Equals(0L) || idVal.Equals(0) || idVal.Equals("");
        if (isNew) _db.Set<T>().Add(entity);
        else _db.Set<T>().Update(entity);
        await _db.SaveChangesAsync();
        await _cache.RemoveAsync($"erp:md:{typeof(T).Name}");
        return entity;
    }

    public async Task DeleteAsync<T>(long id) where T : class
    {
        var entity = await _db.Set<T>().FindAsync(id) ?? throw new Biz_Exception("记录不存在");
        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync();
        await _cache.RemoveAsync($"erp:md:{typeof(T).Name}");
    }

    private static IQueryable<T> ApplyKeyword<T>(IQueryable<T> q, string keyword)
    {
        var skipCols = new System.Collections.Generic.HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
        {
            "Remark", "Detail", "Content", "Payload", "Memo", "Description", "Attach", "Note", "Password", "Token", "Avatar", "Sign", "Data", "Log"
        };
        var strProps = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) && !skipCols.Contains(p.Name))
            .ToList();
        if (strProps.Count == 0) return q;

        // 反射拼 OR Contains，避免每个档案手写查询
        var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
        System.Linq.Expressions.Expression? body = null;
        foreach (var p in strProps)
        {
            var access = System.Linq.Expressions.Expression.Property(param, p);
            var notNull = System.Linq.Expressions.Expression.NotEqual(access, System.Linq.Expressions.Expression.Constant(null, typeof(string)));
            var contains = System.Linq.Expressions.Expression.Call(access, nameof(string.Contains), Type.EmptyTypes, System.Linq.Expressions.Expression.Constant(keyword));
            var and = System.Linq.Expressions.Expression.AndAlso(notNull, contains);
            body = body == null ? and : System.Linq.Expressions.Expression.OrElse(body, and);
        }
        var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body!, param);
        return q.Where(lambda);
    }
}

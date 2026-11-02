using System.Linq.Expressions;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.EFCore;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// HR 模块通用 CRUD 基类：分页列表 / 详情 / 新增 / 修改 / 删除。
/// 子类仅需声明实体类型与路由，无需编写重复代码。
/// 鉴权：继承 [Authorize]，登录即可访问（未加 [Permission_] 故不校验细粒度权限）。
/// </summary>
[ApiController]
[Authorize]
public abstract class HrControllerBase<T> : ControllerBase where T : class, new()
{
    protected readonly I_Repository<T> _repo;
    protected readonly ErpDbContext _db;

    protected HrControllerBase(I_Repository<T> repo, ErpDbContext db)
    {
        _repo = repo;
        _db = db;
    }

    /// <summary>分页列表（支持 keyword 在所有字符串字段模糊匹配）</summary>
    [HttpGet]
    public virtual async Task<Api_Response> Page([FromQuery] Page_Request req)
    {
        var q = ApplyKeyword(_repo.Query(), req.keyword);
        var total = await q.CountAsync();
        var rows = await q
            .OrderByDescending(e => EF.Property<long>(e, "Id"))
            .Skip((req.page - 1) * req.size)
            .Take(req.size)
            .ToListAsync();
        return Api_Response.Ok(new Page_Result { total = total, rows = rows });
    }

    /// <summary>详情</summary>
    [HttpGet("{id:long}")]
    public virtual async Task<Api_Response> Get(long id)
    {
        var e = await _db.Set<T>().FindAsync(id);
        return e == null ? Api_Response.Fail("记录不存在") : Api_Response.Ok(e);
    }

    /// <summary>新增</summary>
    [HttpPost]
    [ActiconLog_Attribute(group = "数据维护", action = "新增")]
    public virtual async Task<Api_Response> Create([FromBody] T entity)
    {
        await BeforeCreateAsync(entity);
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return Api_Response.Ok(entity);
    }

    /// <summary>
    /// 新增前的业务校验钩子。默认放行；子类可覆写以拦截特定资源的创建
    /// （例如 MES 工单仅允许由生产订单派生，禁止手工新增）。
    /// </summary>
    protected virtual Task BeforeCreateAsync(T entity) => Task.CompletedTask;

    /// <summary>全量更新</summary>
    [HttpPut("{id:long}")]
    [ActiconLog_Attribute(group = "数据维护", action = "修改")]
    public virtual async Task<Api_Response> Update(long id, [FromBody] T entity)
    {
        var existing = await _db.Set<T>().FindAsync(id);
        if (existing == null) return Api_Response.Fail("记录不存在");
        _db.Entry(existing).CurrentValues.SetValues(entity);
        await _db.SaveChangesAsync();
        return Api_Response.Ok(existing);
    }

    /// <summary>删除</summary>
    [HttpDelete("{id:long}")]
    [ActiconLog_Attribute(group = "数据维护", action = "删除")]
    public virtual async Task<Api_Response> Delete(long id)
    {
        var e = await _db.Set<T>().FindAsync(id);
        if (e == null) return Api_Response.Fail("记录不存在");
        _repo.Remove(e);
        await _repo.SaveChangesAsync();
        return Api_Response.Ok();
    }

    /// <summary>在所有字符串字段上做 OR 模糊匹配（EF 可翻译为 LIKE）</summary>
    private static IQueryable<T> ApplyKeyword(IQueryable<T> q, string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return q;
        var stringProps = typeof(T).GetProperties()
            .Where(p => p.PropertyType == typeof(string) && p.CanRead)
            .ToList();
        if (stringProps.Count == 0) return q;

        var param = Expression.Parameter(typeof(T), "e");
        Expression? orExpr = null;
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
        var kwConstant = Expression.Constant(keyword, typeof(string));
        foreach (var p in stringProps)
        {
            var propAccess = Expression.Property(param, p);
            var body = Expression.Call(propAccess, containsMethod, kwConstant);
            orExpr = orExpr == null ? body : Expression.OrElse(orExpr, body);
        }
        if (orExpr == null) return q;
        var lambda = Expression.Lambda<Func<T, bool>>(orExpr, param);
        return q.Where(lambda);
    }
}

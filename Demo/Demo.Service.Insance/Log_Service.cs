using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.EFCore;
using Demo.Repository.Interface;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Service.Instance;

[Inject_Attribute]
public class Log_Service : I_Log_Service
{
    private readonly I_Repository<SysOperLog> _oper;
    private readonly I_Repository<SysExLog> _ex;
    private readonly ErpDbContext _db;

    public Log_Service(I_Repository<SysOperLog> oper, I_Repository<SysExLog> ex, ErpDbContext db)
    {
        _oper = oper;
        _ex = ex;
        _db = db;
    }

    public async Task Add_ActionAsync(SysOperLog log)
    {
        await _oper.AddAsync(log);
        await _oper.SaveChangesAsync();
    }

    public async Task Add_ExceptionAsync(SysExLog log)
    {
        await _ex.AddAsync(log);
        await _ex.SaveChangesAsync();
    }

    public async Task<object> PageOperAsync(Page_Request req)
    {
        var q = _oper.Query();
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => (d.Module ?? "").Contains(req.keyword) || (d.Action ?? "").Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id).Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();

        // 联用户表，补操作人姓名
        var userIds = rows.Where(d => d.UserId.HasValue).Select(d => d.UserId!.Value).Distinct().ToList();
        var userMap = await _db.SysUsers.Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.RealName ?? u.Username);
        var list = rows.Select(d => new
        {
            d.Id, d.UserId, d.Module, d.Action, d.Detail, d.OperTime,
            UserName = d.UserId.HasValue && userMap.TryGetValue(d.UserId.Value, out var un) ? un : ""
        }).ToList<object>();
        return new Page_Result { total = total, rows = list };
    }

    public async Task<object> PageExAsync(Page_Request req)
    {
        var q = _ex.Query();
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => (d.RequestPath ?? "").Contains(req.keyword) || (d.Message ?? "").Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id).Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Page_Result { total = total, rows = rows };
    }
}

using System.Text.Json;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Demo.Service.Instance;

/// <summary>
/// 支付回调幂等：Redis SET NX 短锁 + pay_notify.uk_notify_no 唯一约束，重复通知直接返回成功。
/// </summary>
[Inject_Attribute]
public class Pay_Service : I_Pay_Service
{
    private readonly ErpDbContext _db;
    private readonly Redis_Cache _cache;
    private readonly I_Finance_Service _fin;
    private readonly IConfiguration _cfg;

    public Pay_Service(ErpDbContext db, Redis_Cache cache, I_Finance_Service fin, IConfiguration cfg)
    {
        _db = db;
        _cache = cache;
        _fin = fin;
        _cfg = cfg;
    }

    public async Task<object> NotifyAsync(Pay_Notify_Request req)
    {
        if (string.IsNullOrWhiteSpace(req.notifyNo)) throw new Biz_Exception("缺少 notifyNo");
        var secret = _cfg["Pay:Secret"] ?? "erp-pay-secret";
        var expect = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes($"{req.notifyNo}|{req.receivableId}|{req.amount}|{secret}"))).ToLowerInvariant();
        if (!string.IsNullOrEmpty(req.sign) && !string.Equals(req.sign, expect, StringComparison.OrdinalIgnoreCase))
            throw new Biz_Exception("验签失败");

        var lockKey = $"erp:pay:lock:{req.notifyNo}";
        var locked = await _cache.TryLockAsync(lockKey, TimeSpan.FromMinutes(5));
        var existed = await _db.PayNotifies.AsNoTracking().FirstOrDefaultAsync(d => d.NotifyNo == req.notifyNo);
        if (existed != null)
            return new { duplicated = true, status = existed.Status };

        if (!locked)
        {
            existed = await _db.PayNotifies.AsNoTracking().FirstOrDefaultAsync(d => d.NotifyNo == req.notifyNo);
            if (existed != null) return new { duplicated = true, status = existed.Status };
            throw new Biz_Exception("回调处理中，请稍后重试");
        }

        var row = new PayNotify
        {
            NotifyNo = req.notifyNo,
            Channel = req.channel,
            BizNo = req.bizNo,
            ReceivableId = req.receivableId,
            Amount = req.amount,
            Status = 0,
            Payload = JsonSerializer.Serialize(req),
            CreateTime = DateTime.Now
        };
        try
        {
            _db.PayNotifies.Add(row);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return new { duplicated = true, status = 1 };
        }

        try
        {
            var ar = await _db.FinReceivables.FindAsync(req.receivableId) ?? throw new Biz_Exception("应收单不存在");
            await _fin.WriteoffReceiptAsync(new Receipt_Writeoff_Request
            {
                customerId = ar.CustomerId,
                receivableId = ar.Id,
                amount = req.amount,
                receiptDate = DateOnly.FromDateTime(DateTime.Today)
            });
            row.Status = 1;
            row.UpdateTime = DateTime.Now;
            await _db.SaveChangesAsync();
            return new { duplicated = false, status = 1 };
        }
        catch
        {
            row.Status = 2;
            row.UpdateTime = DateTime.Now;
            await _db.SaveChangesAsync();
            throw;
        }
    }
}

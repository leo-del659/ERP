using StackExchange.Redis;
using System.Text.Json;

namespace Demo.Infrastructure.Tools;

public class Redis_Cache
{
    private readonly IDatabase _db;
    private static readonly JsonSerializerOptions JsonOpt = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public Redis_Cache(IConnectionMultiplexer mux)
    {
        _db = mux.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var v = await _db.StringGetAsync(key);
        if (v.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>(v!, JsonOpt);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
    {
        var json = JsonSerializer.Serialize(value, JsonOpt);
        await _db.StringSetAsync(key, json, ttl ?? TimeSpan.FromMinutes(10));
    }

    public Task RemoveAsync(string key) => _db.KeyDeleteAsync(key);

    /// <summary>
    /// 读缓存，未命中则执行 factory 并写回，TTL 默认 10 分钟。
    /// </summary>
    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan? ttl = null)
    {
        var cached = await GetAsync<T>(key);
        if (cached is not null) return cached;
        var value = await factory();
        if (value is not null) await SetAsync(key, value, ttl);
        return value;
    }

    /// <summary>SET NX，用于支付回调 / 消息消费幂等锁。</summary>
    public Task<bool> TryLockAsync(string key, TimeSpan ttl)
        => _db.StringSetAsync(key, "1", ttl, When.NotExists);

    public async Task<long> IncrAsync(string key, TimeSpan? ttl = null)
    {
        var n = await _db.StringIncrementAsync(key);
        if (n == 1 && ttl.HasValue) await _db.KeyExpireAsync(key, ttl);
        return n;
    }
}

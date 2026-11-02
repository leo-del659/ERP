namespace Demo.Infrastructure.Tools;

public class BillNo_Helper
{
    private readonly Redis_Cache _redis;

    public BillNo_Helper(Redis_Cache redis)
    {
        _redis = redis;
    }

    public async Task<string> NextAsync(string prefix)
    {
        var day = DateTime.Now.ToString("yyyyMMdd");
        var n = await _redis.IncrAsync($"erp:bill:{prefix}:{day}", TimeSpan.FromDays(2));
        return $"{prefix}{day}{n:0000}";
    }
}

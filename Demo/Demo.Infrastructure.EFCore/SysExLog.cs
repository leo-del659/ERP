namespace Demo.Infrastructure.EFCore;

/// <summary>
/// 异常日志
/// </summary>
public partial class SysExLog
{
    public long Id { get; set; }

    public string? RequestPath { get; set; }

    public string? Message { get; set; }

    public string? StackTrace { get; set; }

    public DateTime? CreateTime { get; set; }
}

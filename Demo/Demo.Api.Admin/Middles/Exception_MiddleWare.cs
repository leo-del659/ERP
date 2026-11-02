using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.EFCore;
using Demo.Service.Interface;
using System.Net;
using System.Text.Json;

namespace Demo.Api.Admin.Middles;

public class Exception_MiddleWare
{
    private readonly RequestDelegate _next;

    public Exception_MiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await WriteFileAsync(context, ex, configuration);
            try
            {
                await log.Add_ExceptionAsync(new SysExLog
                {
                    RequestPath = context.Request.Path,
                    Message = ex.Message,
                    StackTrace = ex.ToString().Length > 4000 ? ex.ToString()[..4000] : ex.ToString(),
                    CreateTime = DateTime.Now
                });
            }
            catch { }

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task WriteFileAsync(HttpContext context, Exception ex, IConfiguration configuration)
    {
        var log_path = configuration["Logs:Error_Log"];
        if (string.IsNullOrWhiteSpace(log_path)) return;
        try
        {
            Directory.CreateDirectory(log_path);
            var fullPath = Path.Combine(log_path, DateTime.Now.ToString("yyyyMMdd") + ".txt");
            await File.AppendAllTextAsync(fullPath, $"时间:{DateTime.Now}\r\n地址：{context.Request.Path}\r\n{ex}\r\n");
        }
        catch { }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        if (context.Response.HasStarted) return Task.CompletedTask;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        var result = ex is Biz_Exception be
            ? Api_Response.Fail(be.Message, bizCode: be.BizCode)
            : Api_Response.Error("操作异常");
        return context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}

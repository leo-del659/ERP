using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.EFCore;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text;

namespace Demo.Api.Admin.Filters;

public class Action_Filter : IAsyncActionFilter
{
    private readonly IConfiguration _configuration;
    private readonly I_Log_Service _log_Service;
    private readonly I_Auth_Service _auth;

    public Action_Filter(IConfiguration configuration, I_Log_Service log_Service, I_Auth_Service auth)
    {
        _configuration = configuration;
        _log_Service = log_Service;
        _auth = auth;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executedCtx = await next();
        var result = executedCtx.Result as ObjectResult;
        var attr = context.ActionDescriptor.EndpointMetadata.OfType<ActiconLog_Attribute>().FirstOrDefault();
        if (attr == null) return;

        var paras = "";
        if (HttpMethods.IsGet(context.HttpContext.Request.Method))
        {
            foreach (var item in context.HttpContext.Request.Query)
                paras += $"{item.Key}={item.Value};";
        }
        else
        {
            var request = context.HttpContext.Request;
            request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            paras = await reader.ReadToEndAsync();
            paras = paras.Length > 499 ? paras[..499] : paras;
            paras = MaskPasswordJson(paras);
        }

        var res = result?.Value as Api_Response;
        var api = context.HttpContext.Request.Path.ToString();
        var name = context.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        long? uid = null;
        if (!string.IsNullOrWhiteSpace(name))
            uid = _auth.FindByName(name)?.Id;

        var message = $"时间：{DateTime.Now}\r\nURL:{api}\r\n类型：{attr.group}\r\n行为：{attr.action}\r\nMethod:{context.HttpContext.Request.Method}\r\n参数：{paras}\r\n结果：{res?.code}\r\n";
        var path = _configuration["Logs:Action_Log"];
        if (!string.IsNullOrWhiteSpace(path))
        {
            Directory.CreateDirectory(path);
            await File.AppendAllTextAsync(Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".txt"), message);
        }

        try
        {
            await _log_Service.Add_ActionAsync(new SysOperLog
            {
                UserId = uid,
                Module = attr.group,
                Action = attr.action,
                Detail = (api + " " + paras).Length > 500 ? (api + " " + paras)[..500] : api + " " + paras,
                OperTime = DateTime.Now
            });
        }
        catch
        {
            // 操作日志写入失败不应影响正常业务响应
        }
    }

    private static string MaskPasswordJson(string jsonStr)
    {
        var start_index = jsonStr.IndexOf("password", StringComparison.OrdinalIgnoreCase);
        if (start_index < 0) return jsonStr;
        var s = jsonStr[(start_index + 12)..];
        var stop_index = s.IndexOf('"');
        if (stop_index < 0) return jsonStr;
        return jsonStr[..(start_index + 12)] + "******" + s[stop_index..];
    }
}

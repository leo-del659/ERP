using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Demo.Api.Admin.Filters;

public class Permission_Filter : IAsyncActionFilter
{
    private readonly I_Auth_Service _auth;

    public Permission_Filter(I_Auth_Service auth)
    {
        _auth = auth;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var attr = context.ActionDescriptor.EndpointMetadata.OfType<Permission_Attribute>().FirstOrDefault();
        if (attr == null)
        {
            await next();
            return;
        }

        var name = context.HttpContext.User.FindFirstValue(ClaimTypes.Name)
                   ?? context.HttpContext.User.FindFirstValue("unique_name");
        if (string.IsNullOrWhiteSpace(name))
        {
            context.Result = new JsonResult(Api_Response.Fail("未登录")) { StatusCode = 401 };
            return;
        }

        var user = _auth.FindByName(name);
        if (user == null)
        {
            context.Result = new JsonResult(Api_Response.Fail("用户无效")) { StatusCode = 401 };
            return;
        }

        var paths = await _auth.GetPathsAsync(user.Id);
        if (!paths.Any(p => Has_Permission(p, attr.Key)))
        {
            context.Result = new JsonResult(Api_Response.Fail("无访问权限：" + attr.Key)) { StatusCode = 403 };
            return;
        }

        await next();
    }

    private static bool Has_Permission(string path, string key)
    {
        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(key)) return false;
        path = path.Trim();
        key = key.Trim();
        if (string.Equals(path, key, StringComparison.OrdinalIgnoreCase)) return true;
        if (path.Length > key.Length && path.StartsWith(key, StringComparison.OrdinalIgnoreCase) && path[key.Length] == '/') return true;
        if (key.Length > path.Length && key.StartsWith(path, StringComparison.OrdinalIgnoreCase) && key[path.Length] == '/') return true;
        return false;
    }
}

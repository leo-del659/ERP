using System.Reflection;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Authorize]
[Permission_("/md")]
[Route("api/[controller]")]
public class MdController : ControllerBase
{
    private static readonly Dictionary<string, Type> Types = new(StringComparer.OrdinalIgnoreCase)
    {
        ["material"] = typeof(MdMaterial),
        ["customer"] = typeof(MdCustomer),
        ["supplier"] = typeof(MdSupplier),
        ["warehouse"] = typeof(MdWarehouse),
        ["account"] = typeof(MdAccount),
        ["bom"] = typeof(MdBom),
        ["user"] = typeof(SysUser),
        ["role"] = typeof(SysRole),
        ["menu"] = typeof(SysMenu),
        ["credit"] = typeof(SoCustomerCredit)
    };

    private readonly I_Md_Service _md;
    private readonly IConfiguration _cfg;
    public MdController(I_Md_Service md, IConfiguration cfg)
    {
        _md = md;
        _cfg = cfg;
    }

    [HttpGet("{entity}")]
    public async Task<Api_Response> Page(string entity, [FromQuery] Page_Request req)
    {
        var type = Resolve(entity);
        var mi = typeof(I_Md_Service).GetMethod(nameof(I_Md_Service.PageAsync))!.MakeGenericMethod(type);
        var data = await (Task<object>)mi.Invoke(_md, new object?[] { req, null })!;
        return Api_Response.Ok(data);
    }

    [HttpPost("{entity}")]
    [ActiconLog_(group = "主数据", action = "保存")]
    public async Task<Api_Response> Save(string entity, [FromBody] JsonElement body)
    {
        var type = Resolve(entity);
        var obj = body.Deserialize(type, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                  ?? throw new Biz_Exception("无法反序列化");
        if (obj is SysUser u && !string.IsNullOrWhiteSpace(u.Password) && !EncryptHelper.IsArgon2Hash(u.Password))
            u.Password = EncryptHelper.HashPassword(u.Password, _cfg["Login:Keys"]);
        var mi = typeof(I_Md_Service).GetMethod(nameof(I_Md_Service.SaveAsync))!.MakeGenericMethod(type);
        var data = await (Task<object>)mi.Invoke(_md, new[] { obj })!;
        return Api_Response.Ok(data);
    }

    [HttpDelete("{entity}/{id:long}")]
    [ActiconLog_(group = "主数据", action = "删除")]
    public async Task<Api_Response> Delete(string entity, long id)
    {
        var type = Resolve(entity);
        var mi = typeof(I_Md_Service).GetMethod(nameof(I_Md_Service.DeleteAsync))!.MakeGenericMethod(type);
        await (Task)mi.Invoke(_md, new object[] { id })!;
        return Api_Response.Ok();
    }

    private static Type Resolve(string entity)
        => Types.TryGetValue(entity, out var t) ? t : throw new Biz_Exception("未知实体 " + entity);
}

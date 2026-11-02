using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly I_Auth_Service _auth;
    public AuthController(I_Auth_Service auth) => _auth = auth;

    [AllowAnonymous]
    [HttpPost("login")]
    [ActiconLog_(group = "系统", action = "登录")]
    // 登录接口按前端约定返回原始数据 {token,user,paths}（不包 Api_Response），
    // 前端 request.ts 对 /api/auth/login 直接返回 response.data，auth.ts 读取 res.token/res.user
    public async Task<object> Login([FromBody] Login_Request req)
        => await _auth.LoginAsync(req);
}

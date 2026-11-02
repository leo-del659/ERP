using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Repository.Interface;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Demo.Service.Instance;

[Inject_Attribute]
public class Auth_Service : I_Auth_Service
{
    private readonly I_Repository<SysUser> _users;
    private readonly I_Repository<SysUserRole> _userRoles;
    private readonly I_Repository<SysRole> _roles;
    private readonly I_Repository<SysDeptRole> _deptRoles;
    private readonly I_Repository<SysDeptRoleMenu> _drm;
    private readonly I_Repository<SysMenu> _menus;
    private readonly JWT_Helper _jwt;
    private readonly IConfiguration _cfg;
    private readonly Redis_Cache _cache;

    public Auth_Service(
        I_Repository<SysUser> users,
        I_Repository<SysUserRole> userRoles,
        I_Repository<SysRole> roles,
        I_Repository<SysDeptRole> deptRoles,
        I_Repository<SysDeptRoleMenu> drm,
        I_Repository<SysMenu> menus,
        JWT_Helper jwt,
        IConfiguration cfg,
        Redis_Cache cache)
    {
        _users = users;
        _userRoles = userRoles;
        _roles = roles;
        _deptRoles = deptRoles;
        _drm = drm;
        _menus = menus;
        _jwt = jwt;
        _cfg = cfg;
        _cache = cache;
    }

    public SysUser? FindByName(string username)
        => _users.Query(d => d.Username == username).FirstOrDefault();

    public async Task<object> LoginAsync(Login_Request req)
    {
        var user = FindByName(req.username) ?? throw new Biz_Exception("账号或密码错误");
        if (user.Status == 0) throw new Biz_Exception("账号已停用");

        var pepper = _cfg["Login:Keys"];
        var outcome = EncryptHelper.Verify(req.password, user.Password, pepper: pepper);
        if (outcome != EncryptHelper.PasswordVerifyOutcome.Argon2Matched)
            throw new Biz_Exception("账号或密码错误");

        var roleIds = _userRoles.Query(d => d.UserId == user.Id).Select(d => d.RoleId).ToList();
        var roleCode = _roles.Query(d => roleIds.Contains(d.Id)).Select(d => d.RoleCode).FirstOrDefault() ?? "user";
        var token = _jwt.GenerateToken(user.Username, roleCode);
        var paths = await GetPathsAsync(user.Id);
        return new { token, user = new { user.Id, user.Username, user.RealName, roleCode }, paths };
    }

    public async Task<List<string>> GetPathsAsync(long userId)
    {
        var cacheKey = $"erp:perm:{userId}";
        var cached = await _cache.GetAsync<List<string>>(cacheKey);
        if (cached != null) return cached;

        var roleIds = _userRoles.Query(d => d.UserId == userId).Select(d => d.RoleId).ToList();
        if (_roles.Query(d => roleIds.Contains(d.Id) && d.RoleCode == "admin").Any())
        {
            var all = _menus.Query(d => d.IsBan != 1).Select(d => d.Path!).Where(p => p != null).ToList();
            await _cache.SetAsync(cacheKey, all, TimeSpan.FromMinutes(5));
            return all;
        }

        var deptRoleIds = _deptRoles.Query(d => roleIds.Contains(d.RoleId) && d.IsBan != 1).Select(d => d.Id).ToList();
        var menuIds = _drm.Query(d => deptRoleIds.Contains(d.DeptRoleId)).Select(d => d.MenuId).ToList();
        var paths = _menus.Query(d => menuIds.Contains(d.Id) && d.IsBan != 1)
            .Select(d => d.Path!).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
        await _cache.SetAsync(cacheKey, paths, TimeSpan.FromMinutes(5));
        return paths;
    }
}

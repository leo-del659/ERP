using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_leave
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/leave")]
[Route("api/hr/leave")]
public class HrLeaveController : HrControllerBase<HrLeave>
{
    public HrLeaveController(I_Repository<HrLeave> repo, ErpDbContext db) : base(repo, db) { }
}

using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_overtime
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/overtime")]
[Route("api/hr/overtime")]
public class HrOvertimeController : HrControllerBase<HrOvertime>
{
    public HrOvertimeController(I_Repository<HrOvertime> repo, ErpDbContext db) : base(repo, db) { }
}

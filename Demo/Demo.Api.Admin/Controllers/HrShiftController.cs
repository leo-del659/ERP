using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_shift
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/shift")]
[Route("api/hr/shift")]
public class HrShiftController : HrControllerBase<HrShift>
{
    public HrShiftController(I_Repository<HrShift> repo, ErpDbContext db) : base(repo, db) { }
}

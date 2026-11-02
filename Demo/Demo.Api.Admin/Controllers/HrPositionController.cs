using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_position
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/position")]
[Route("api/hr/position")]
public class HrPositionController : HrControllerBase<HrPosition>
{
    public HrPositionController(I_Repository<HrPosition> repo, ErpDbContext db) : base(repo, db) { }
}

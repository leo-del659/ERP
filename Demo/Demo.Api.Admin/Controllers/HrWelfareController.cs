using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_welfare
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/welfare")]
[Route("api/hr/welfare")]
public class HrWelfareController : HrControllerBase<HrWelfare>
{
    public HrWelfareController(I_Repository<HrWelfare> repo, ErpDbContext db) : base(repo, db) { }
}

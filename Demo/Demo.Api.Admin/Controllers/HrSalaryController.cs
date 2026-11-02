using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_salary
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/salary")]
[Route("api/hr/salary")]
public class HrSalaryController : HrControllerBase<HrSalary>
{
    public HrSalaryController(I_Repository<HrSalary> repo, ErpDbContext db) : base(repo, db) { }
}

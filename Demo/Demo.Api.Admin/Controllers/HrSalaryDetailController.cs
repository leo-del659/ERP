using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_salary_detail
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/salary-detail")]
[Route("api/hr/salary-detail")]
public class HrSalaryDetailController : HrControllerBase<HrSalaryDetail>
{
    public HrSalaryDetailController(I_Repository<HrSalaryDetail> repo, ErpDbContext db) : base(repo, db) { }
}

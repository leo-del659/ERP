using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_salary_item
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/salary-item")]
[Route("api/hr/salary-item")]
public class HrSalaryItemController : HrControllerBase<HrSalaryItem>
{
    public HrSalaryItemController(I_Repository<HrSalaryItem> repo, ErpDbContext db) : base(repo, db) { }
}

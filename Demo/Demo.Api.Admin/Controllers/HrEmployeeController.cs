using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_employee
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/employee")]
[Route("api/hr/employee")]
public class HrEmployeeController : HrControllerBase<HrEmployee>
{
    public HrEmployeeController(I_Repository<HrEmployee> repo, ErpDbContext db) : base(repo, db) { }
}

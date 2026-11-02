using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_employee_change
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/employee-change")]
[Route("api/hr/employee-change")]
public class HrEmployeeChangeController : HrControllerBase<HrEmployeeChange>
{
    public HrEmployeeChangeController(I_Repository<HrEmployeeChange> repo, ErpDbContext db) : base(repo, db) { }
}

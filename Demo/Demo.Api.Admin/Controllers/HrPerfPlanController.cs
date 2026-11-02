using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_perf_plan
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/perf-plan")]
[Route("api/hr/perf-plan")]
public class HrPerfPlanController : HrControllerBase<HrPerfPlan>
{
    public HrPerfPlanController(I_Repository<HrPerfPlan> repo, ErpDbContext db) : base(repo, db) { }
}

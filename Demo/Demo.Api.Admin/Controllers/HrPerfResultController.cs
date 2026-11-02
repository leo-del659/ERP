using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_perf_result
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/perf-result")]
[Route("api/hr/perf-result")]
public class HrPerfResultController : HrControllerBase<HrPerfResult>
{
    public HrPerfResultController(I_Repository<HrPerfResult> repo, ErpDbContext db) : base(repo, db) { }
}

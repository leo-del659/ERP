using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_perf_indicator
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/perf-indicator")]
[Route("api/hr/perf-indicator")]
public class HrPerfIndicatorController : HrControllerBase<HrPerfIndicator>
{
    public HrPerfIndicatorController(I_Repository<HrPerfIndicator> repo, ErpDbContext db) : base(repo, db) { }
}

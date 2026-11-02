using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_perf_detail
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/perf-detail")]
[Route("api/hr/perf-detail")]
public class HrPerfDetailController : HrControllerBase<HrPerfDetail>
{
    public HrPerfDetailController(I_Repository<HrPerfDetail> repo, ErpDbContext db) : base(repo, db) { }
}

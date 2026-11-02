using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_position_quota
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/position-quota")]
[Route("api/hr/position-quota")]
public class HrPositionQuotaController : HrControllerBase<HrPositionQuota>
{
    public HrPositionQuotaController(I_Repository<HrPositionQuota> repo, ErpDbContext db) : base(repo, db) { }
}

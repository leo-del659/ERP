using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_recruit_plan
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/recruit-plan")]
[Route("api/hr/recruit-plan")]
public class HrRecruitPlanController : HrControllerBase<HrRecruitPlan>
{
    public HrRecruitPlanController(I_Repository<HrRecruitPlan> repo, ErpDbContext db) : base(repo, db) { }
}

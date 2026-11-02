using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_social_rule
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/social-rule")]
[Route("api/hr/social-rule")]
public class HrSocialRuleController : HrControllerBase<HrSocialRule>
{
    public HrSocialRuleController(I_Repository<HrSocialRule> repo, ErpDbContext db) : base(repo, db) { }
}

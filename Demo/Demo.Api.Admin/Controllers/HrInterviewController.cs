using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_interview
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/interview")]
[Route("api/hr/interview")]
public class HrInterviewController : HrControllerBase<HrInterview>
{
    public HrInterviewController(I_Repository<HrInterview> repo, ErpDbContext db) : base(repo, db) { }
}

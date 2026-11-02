using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_candidate
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/candidate")]
[Route("api/hr/candidate")]
public class HrCandidateController : HrControllerBase<HrCandidate>
{
    public HrCandidateController(I_Repository<HrCandidate> repo, ErpDbContext db) : base(repo, db) { }
}

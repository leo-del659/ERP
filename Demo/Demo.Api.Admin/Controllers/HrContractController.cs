using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_contract
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/contract")]
[Route("api/hr/contract")]
public class HrContractController : HrControllerBase<HrContract>
{
    public HrContractController(I_Repository<HrContract> repo, ErpDbContext db) : base(repo, db) { }
}

using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_tax_bracket
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/tax-bracket")]
[Route("api/hr/tax-bracket")]
public class HrTaxBracketController : HrControllerBase<HrTaxBracket>
{
    public HrTaxBracketController(I_Repository<HrTaxBracket> repo, ErpDbContext db) : base(repo, db) { }
}

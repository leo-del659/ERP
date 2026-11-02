using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_offer
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/offer")]
[Route("api/hr/offer")]
public class HrOfferController : HrControllerBase<HrOffer>
{
    public HrOfferController(I_Repository<HrOffer> repo, ErpDbContext db) : base(repo, db) { }
}

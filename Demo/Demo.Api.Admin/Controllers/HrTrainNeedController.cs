using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_train_need
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/train-need")]
[Route("api/hr/train-need")]
public class HrTrainNeedController : HrControllerBase<HrTrainNeed>
{
    public HrTrainNeedController(I_Repository<HrTrainNeed> repo, ErpDbContext db) : base(repo, db) { }
}

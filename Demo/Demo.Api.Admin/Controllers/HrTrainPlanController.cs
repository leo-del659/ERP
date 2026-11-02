using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_train_plan
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/train-plan")]
[Route("api/hr/train-plan")]
public class HrTrainPlanController : HrControllerBase<HrTrainPlan>
{
    public HrTrainPlanController(I_Repository<HrTrainPlan> repo, ErpDbContext db) : base(repo, db) { }
}

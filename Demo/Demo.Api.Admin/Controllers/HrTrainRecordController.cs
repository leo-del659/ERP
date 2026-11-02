using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_train_record
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/train-record")]
[Route("api/hr/train-record")]
public class HrTrainRecordController : HrControllerBase<HrTrainRecord>
{
    public HrTrainRecordController(I_Repository<HrTrainRecord> repo, ErpDbContext db) : base(repo, db) { }
}

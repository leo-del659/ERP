using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_train_expense
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/train-expense")]
[Route("api/hr/train-expense")]
public class HrTrainExpenseController : HrControllerBase<HrTrainExpense>
{
    public HrTrainExpenseController(I_Repository<HrTrainExpense> repo, ErpDbContext db) : base(repo, db) { }
}

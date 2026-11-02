using System.Threading.Tasks;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 生产制造模块：mes_quality_check
/// </summary>
[ApiController]
[Authorize]
[Permission_("/mes/quality-check")]
[Route("api/mes/quality-check")]
public class MesQualityCheckController : HrControllerBase<MesQualityCheck>
{
    public MesQualityCheckController(I_Repository<MesQualityCheck> repo, ErpDbContext db) : base(repo, db) { }
}
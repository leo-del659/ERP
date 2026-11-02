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
/// 生产制造模块：mes_work_report
/// </summary>
[ApiController]
[Authorize]
[Permission_("/mes/work-report")]
[Route("api/mes/work-report")]
public class MesWorkReportController : HrControllerBase<MesWorkReport>
{
    public MesWorkReportController(I_Repository<MesWorkReport> repo, ErpDbContext db) : base(repo, db) { }
}
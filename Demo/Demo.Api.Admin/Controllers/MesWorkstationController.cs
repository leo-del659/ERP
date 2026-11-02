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
/// 生产制造模块：mes_workstation
/// </summary>
[ApiController]
[Authorize]
[Permission_("/mes/workstation")]
[Route("api/mes/workstation")]
public class MesWorkstationController : HrControllerBase<MesWorkstation>
{
    public MesWorkstationController(I_Repository<MesWorkstation> repo, ErpDbContext db) : base(repo, db) { }
}
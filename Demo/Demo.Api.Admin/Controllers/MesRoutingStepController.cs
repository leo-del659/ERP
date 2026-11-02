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
/// 生产制造模块：mes_routing_step
/// </summary>
[ApiController]
[Authorize]
[Permission_("/mes/routing-step")]
[Route("api/mes/routing-step")]
public class MesRoutingStepController : HrControllerBase<MesRoutingStep>
{
    public MesRoutingStepController(I_Repository<MesRoutingStep> repo, ErpDbContext db) : base(repo, db) { }
}
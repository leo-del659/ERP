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
/// 生产制造模块：mes_routing
/// </summary>
[ApiController]
[Authorize]
[Permission_("/mes/routing")]
[Route("api/mes/routing")]
public class MesRoutingController : HrControllerBase<MesRouting>
{
    public MesRoutingController(I_Repository<MesRouting> repo, ErpDbContext db) : base(repo, db) { }
}
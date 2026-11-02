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
/// 生产制造模块：mes_production_order
/// </summary>
[ApiController]
[Authorize]
[Permission_("/mes/production-order")]
[Route("api/mes/production-order")]
public class MesProductionOrderController : HrControllerBase<MesProductionOrder>
{
    public MesProductionOrderController(I_Repository<MesProductionOrder> repo, ErpDbContext db) : base(repo, db) { }
}
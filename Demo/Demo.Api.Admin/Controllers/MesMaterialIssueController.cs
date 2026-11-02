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
/// 生产制造模块：mes_material_issue
/// </summary>
[ApiController]
[Authorize]
[Permission_("/mes/material-issue")]
[Route("api/mes/material-issue")]
public class MesMaterialIssueController : HrControllerBase<MesMaterialIssue>
{
    public MesMaterialIssueController(I_Repository<MesMaterialIssue> repo, ErpDbContext db) : base(repo, db) { }
}
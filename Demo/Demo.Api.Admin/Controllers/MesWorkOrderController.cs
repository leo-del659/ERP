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
/// 生产制造模块：mes_work_order
/// </summary>
[ApiController]
[Authorize]
[Permission_("/mes/work-order")]
[Route("api/mes/work-order")]
public class MesWorkOrderController : HrControllerBase<MesWorkOrder>
{
    public MesWorkOrderController(I_Repository<MesWorkOrder> repo, ErpDbContext db) : base(repo, db) { }

    /// <summary>
    /// 新增前校验：MES 工单仅允许由生产订单派生。
    /// 规则：必须关联生产订单、source=2、生产订单存在、同一订单不得重复派生。
    /// </summary>
    protected override async Task BeforeCreateAsync(MesWorkOrder entity)
    {
        if (entity.PrOrderId == null || entity.Source != 2)
            throw new Biz_Exception("MES 工单仅允许由生产订单派生，请使用「拉取生产订单」生成，不支持手工新增");

        if (!await _db.PrOrders.AnyAsync(p => p.Id == entity.PrOrderId.Value))
            throw new Biz_Exception("关联的生产订单不存在");

        if (await _db.MesWorkOrders.AnyAsync(w => w.PrOrderId == entity.PrOrderId.Value))
            throw new Biz_Exception("该生产订单已生成 MES 工单，请勿重复创建");
    }
}
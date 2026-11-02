using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.Dto.Response;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Attributes;
using Demo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Admin.Controllers;

/// <summary>
/// 人事模块：hr_schedule
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/schedule")]
[Route("api/hr/schedule")]
public class HrScheduleController : HrControllerBase<HrSchedule>
{
    public HrScheduleController(I_Repository<HrSchedule> repo, ErpDbContext db) : base(repo, db) { }

    /// <summary>
    /// 分页列表：附加排班名称（班次名称 + 排班日期），供考勤等页面引用显示
    /// </summary>
    [HttpGet]
    public override async Task<Api_Response> Page([FromQuery] Page_Request req)
    {
        var q = _db.HrSchedules.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(s => (s.Remark ?? "").Contains(req.keyword));
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(e => e.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();

        var shiftIds = rows.Select(r => r.ShiftId).Distinct().ToList();
        var shiftMap = await _db.HrShifts.Where(s => shiftIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.ShiftName);

        var result = rows.Select(r => new
        {
            r.Id,
            r.EmpId,
            r.ShiftId,
            r.WorkDate,
            r.Remark,
            r.CreateTime,
            ShiftName = shiftMap.TryGetValue(r.ShiftId, out var sn) ? sn : "",
            ScheduleName = HrAttendanceController.BuildScheduleName(
                shiftMap.TryGetValue(r.ShiftId, out var sn2) ? sn2 : "", r.WorkDate)
        }).ToList();
        return Api_Response.Ok(new Page_Result { total = total, rows = result });
    }
}

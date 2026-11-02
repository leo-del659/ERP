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
/// 人事模块：hr_attendance
/// </summary>
[ApiController]
[Authorize]
[Permission_("/hr/attendance")]
[Route("api/hr/attendance")]
public class HrAttendanceController : HrControllerBase<HrAttendance>
{
    public HrAttendanceController(I_Repository<HrAttendance> repo, ErpDbContext db) : base(repo, db) { }

    /// <summary>
    /// 分页列表：附加排班名称（班次名称 + 排班日期），避免前端显示排班ID
    /// </summary>
    [HttpGet]
    public override async Task<Api_Response> Page([FromQuery] Page_Request req)
    {
        var q = _db.HrAttendances.AsQueryable();
        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(e => e.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();

        var ids = rows.Where(r => r.ScheduleId.HasValue).Select(r => r.ScheduleId!.Value).Distinct().ToList();
        var map = await (from s in _db.HrSchedules
                         join sh in _db.HrShifts on s.ShiftId equals sh.Id
                         where ids.Contains(s.Id)
                         select new { s.Id, s.WorkDate, sh.ShiftName }).ToListAsync();
        var nameMap = map.ToDictionary(x => x.Id, x => BuildScheduleName(x.ShiftName, x.WorkDate));

        var result = rows.Select(r => new
        {
            r.Id,
            r.EmpId,
            r.ScheduleId,
            r.WorkDate,
            r.PunchIn,
            r.PunchOut,
            r.LateMin,
            r.EarlyMin,
            r.Absent,
            r.OvertimeMin,
            r.Status,
            r.CreateTime,
            ScheduleName = r.ScheduleId.HasValue && nameMap.TryGetValue(r.ScheduleId.Value, out var n) ? n : null
        }).ToList();
        return Api_Response.Ok(new Page_Result { total = total, rows = result });
    }

    /// <summary>排班名称 = 班次名称 + 排班日期</summary>
    internal static string BuildScheduleName(string? shiftName, DateOnly? workDate)
        => $"{shiftName ?? ""} {workDate?.ToString("yyyy-MM-dd") ?? ""}".Trim();
}

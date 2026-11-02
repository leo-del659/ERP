using System.Collections.Generic;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Service.Instance;

[Inject_Attribute]
public class Hr_Service : I_Hr_Service
{
    private readonly ErpDbContext _db;
    private readonly I_Finance_Service _fin;
    private readonly BillNo_Helper _no;

    public Hr_Service(ErpDbContext db, I_Finance_Service fin, BillNo_Helper no)
    {
        _db = db;
        _fin = fin;
        _no = no;
    }

    public async Task<object> ProcessEmployeeChangeAsync(long changeId, string? operatorName)
    {
        var chg = await _db.HrEmployeeChanges.FindAsync(changeId) ?? throw new Biz_Exception("异动记录不存在");
        var emp = await _db.HrEmployees.FindAsync(chg.EmpId) ?? throw new Biz_Exception("员工不存在");
        var today = DateOnly.FromDateTime(DateTime.Now);
        var effect = chg.EffectDate ?? today;

        switch (chg.ChangeType)
        {
            case 1: // 入职
                emp.EmpStatus = 1; emp.HireDate = effect; break;
            case 2: // 转正
                emp.EmpStatus = 1; emp.RegularDate = effect; break;
            case 3: // 调动
                if (chg.NewDeptId.HasValue) emp.DeptId = chg.NewDeptId;
                if (chg.NewPosId.HasValue) emp.PosId = chg.NewPosId;
                break;
            case 4: // 晋升
            case 5: // 降职
                if (chg.NewPosId.HasValue) emp.PosId = chg.NewPosId;
                break;
            case 6: // 离职
                emp.EmpStatus = 3; emp.LeaveDate = effect; break;
            case 7: // 复职
                emp.EmpStatus = 1; break;
        }

        // 联动合同：离职解除当前生效合同；调动/晋升/降职标记为变更
        var contracts = await _db.HrContracts
            .Where(c => c.EmpId == chg.EmpId && (c.Status == 1 || c.Status == 2))
            .ToListAsync();
        foreach (var c in contracts)
        {
            if (chg.ChangeType == 6) { c.Status = 4; c.EndDate = effect; }
            else if (chg.ChangeType == 3 || chg.ChangeType == 4 || chg.ChangeType == 5) { c.Status = 3; }
        }

        chg.Operator = operatorName;
        await _db.SaveChangesAsync();
        return new { employee = emp, contracts };
    }

    public async Task<object> CalculateSalaryAsync(int year, int month, List<long>? employeeIds)
    {
        if (month < 1 || month > 12) throw new Biz_Exception("月份不合法");
        var period = $"{year:0000}-{month:00}";
        var start = new DateOnly(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var emps = await _db.HrEmployees
            .Where(e => (e.EmpStatus == 0 || e.EmpStatus == 1) && (employeeIds == null || employeeIds.Contains(e.Id)))
            .ToListAsync();

        var results = new List<object>();
        foreach (var emp in emps)
        {
            if (await _db.HrSalarys.AnyAsync(s => s.EmpId == emp.Id && s.Period == period))
            {
                results.Add(new { salaryNo = "(已核算)", empId = emp.Id, empName = emp.Name, gross = 0m, net = 0m, voucherId = 0L, skipped = true });
                continue;
            }
            await using var tx = await _db.Database.BeginTransactionAsync();
            var baseSal = (await _db.HrEmployeeSalarys
                .Where(x => x.EmpId == emp.Id && x.Status == 1)
                .OrderByDescending(x => x.EffDate)
                .FirstOrDefaultAsync())?.BaseSalary ?? 0m;

            var atts = await _db.HrAttendances
                .Where(a => a.EmpId == emp.Id && a.WorkDate >= start && a.WorkDate <= end)
                .ToListAsync();
            var otMin = atts.Sum(a => a.OvertimeMin ?? 0);
            var absentDays = atts.Count(a => a.Status == 3 || a.Absent == 1);

            var perf = await _db.HrPerfResults
                .Where(p => p.EmpId == emp.Id && p.ApplySalary == 1)
                .OrderByDescending(p => p.CreateTime)
                .FirstOrDefaultAsync();
            var score = perf?.Score ?? 0m;

            var hourly = baseSal / 21.75m / 8m;
            var otPay = Math.Round((decimal)otMin / 60m * hourly * 1.5m, 2);
            var leaveDeduct = Math.Round(absentDays * (baseSal / 21.75m), 2);
            var perfBonus = Math.Round(baseSal * (score / 100m) * 0.1m, 2);
            var gross = Math.Round(baseSal + otPay + perfBonus - leaveDeduct, 2);

            var social = Math.Round(gross * 0.105m, 2);
            var tax = gross > 5000m ? Math.Round((gross - 5000m) * 0.1m, 2) : 0m;
            var net = Math.Round(gross - social - tax, 2);

            var salary = new HrSalary
            {
                SalaryNo = await _no.NextAsync("PAY"),
                Period = period,
                EmpId = emp.Id,
                BaseSalary = baseSal,
                AttendDays = Math.Round((decimal)(DateTime.DaysInMonth(year, month) - absentDays), 1),
                OtPay = otPay,
                LeaveDeduct = leaveDeduct,
                PerfBonus = perfBonus,
                GrossPay = gross,
                SocialPersonal = social,
                Tax = tax,
                NetPay = net,
                Status = 1,
                CreateTime = DateTime.Now
            };
            _db.HrSalarys.Add(salary);
            await _db.SaveChangesAsync(); // 先取 Id 再写明细/凭证

            void AddDetail(string name, decimal amt, sbyte dir)
            {
                if (amt == 0) return;
                _db.HrSalaryDetails.Add(new HrSalaryDetail
                {
                    SalaryId = salary.Id,
                    ItemName = name,
                    Amount = amt,
                    Direction = dir,
                    CreateTime = DateTime.Now
                });
            }
            AddDetail("基本工资", baseSal, 1);
            AddDetail("加班费", otPay, 1);
            AddDetail("绩效奖金", perfBonus, 1);
            AddDetail("请假扣款", -leaveDeduct, -1);
            AddDetail("个人社保", -social, -1);
            AddDetail("个人所得税", -tax, -1);
            await _db.SaveChangesAsync();

            // 生成财务凭证（HR → 总账）：借 应付职工薪酬，贷 个税/社保/银行存款
            var entries = new List<Voucher_Entry_Dto>
            {
                new Voucher_Entry_Dto { accountCode = "2211", direction = "DR", amount = gross, memo = $"应付职工薪酬-{emp.Name}({period})" }
            };
            if (tax > 0) entries.Add(new Voucher_Entry_Dto { accountCode = "2221", direction = "CR", amount = tax, memo = "应交税费-个人所得税" });
            if (social > 0) entries.Add(new Voucher_Entry_Dto { accountCode = "2212", direction = "CR", amount = social, memo = "其他应付款-社会保险费" });
            entries.Add(new Voucher_Entry_Dto { accountCode = "1002", direction = "CR", amount = net, memo = "银行存款-实发工资" });

            var req = new Voucher_Create_Request
            {
                voucherDate = start,
                sourceModule = "HR",
                sourceId = salary.Id,
                entries = entries
            };
            var v = await _fin.CreateVoucherAsync(req) as FinVoucher ?? throw new Biz_Exception("生成凭证失败");
            salary.VoucherId = v.Id;
            salary.Status = 3;
            _db.HrSalarys.Update(salary);
            await _db.SaveChangesAsync();

            results.Add(new { salaryNo = salary.SalaryNo, empId = emp.Id, empName = emp.Name, gross, net, voucherId = v.Id });
            await tx.CommitAsync();
        }
        return results;
    }

    public async Task<object> ApproveLeaveAsync(long leaveId, string? operatorName)
    {
        var leave = await _db.HrLeaves.FindAsync(leaveId) ?? throw new Biz_Exception("请假单不存在");
        if (leave.Status == 2 || leave.Status == 3) return leave; // 已处理，幂等
        leave.Status = 2;
        if (leave.StartDate.HasValue && leave.EndDate.HasValue)
        {
            for (var d = leave.StartDate.Value; d <= leave.EndDate.Value; d = d.AddDays(1))
            {
                _db.HrAttendances.Add(new HrAttendance
                {
                    EmpId = leave.EmpId,
                    WorkDate = d,
                    Absent = 1,
                    Status = 3,
                    CreateTime = DateTime.Now
                });
            }
        }
        await _db.SaveChangesAsync();
        return leave;
    }

    public async Task<object> ApproveOvertimeAsync(long overtimeId, string? operatorName)
    {
        var ot = await _db.HrOvertimes.FindAsync(overtimeId) ?? throw new Biz_Exception("加班单不存在");
        if (ot.Status == 2 || ot.Status == 3) return ot; // 已处理，幂等
        ot.Status = 2;
        if (ot.OtDate.HasValue)
        {
            var minutes = (int)Math.Round((ot.Hours ?? 0) * 60);
            _db.HrAttendances.Add(new HrAttendance
            {
                EmpId = ot.EmpId,
                WorkDate = ot.OtDate,
                OvertimeMin = minutes,
                Status = 4,
                CreateTime = DateTime.Now
            });
        }
        await _db.SaveChangesAsync();
        return ot;
    }
}

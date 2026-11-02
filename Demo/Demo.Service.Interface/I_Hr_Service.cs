using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto.Request;

namespace Demo.Service.Interface;

/// <summary>
/// 人事模块业务级联服务：异动联动合同、考勤联动薪资、薪资自动生成财务凭证。
/// </summary>
[Provider_Attribute]
public interface I_Hr_Service
{
    /// <summary>员工异动处理：联动员工状态与合同状态（离职解除、调动/晋升/降职变更合同）。</summary>
    Task<object> ProcessEmployeeChangeAsync(long changeId, string? operatorName);

    /// <summary>薪资核算：定薪 + 考勤(加班/缺勤) + 绩效 自动算薪，并生成财务凭证传递至总账。</summary>
    Task<object> CalculateSalaryAsync(int year, int month, List<long>? employeeIds);

    /// <summary>请假审批通过：联动写入考勤缺勤记录，供薪资核算扣款。</summary>
    Task<object> ApproveLeaveAsync(long leaveId, string? operatorName);

    /// <summary>加班审批通过：联动写入考勤加班记录，供薪资核算计加班费。</summary>
    Task<object> ApproveOvertimeAsync(long overtimeId, string? operatorName);
}

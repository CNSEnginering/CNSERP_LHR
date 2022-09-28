using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Attendance.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.Attendance
{
    public interface IAttendanceAppService : IApplicationService
    {
        Task<string> UpdateAttendance(CreateOrEditAttendanceDetailDto input);

        EmployeeDataForAttendanceDto EmployeeDataForAttendance(int employeeID, DateTime attendanceDate);

        string ScheduleAttendance(DateTime fromDate, DateTime toDate);

    }
}
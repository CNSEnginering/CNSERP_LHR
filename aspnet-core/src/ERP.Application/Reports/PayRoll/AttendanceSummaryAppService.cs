using Abp.Authorization;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.PayRoll.Attendance;
using ERP.PayRoll.Employees;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_AttendanceReports)]
    public class AttendanceSummaryAppService : ERPReportAppServiceBase, IAttendanceSummaryAppService
    {
        private readonly IRepository<AttendanceDetail> _attendanceRepository;
        private readonly IRepository<Employees> _employeesRepository;

        public AttendanceSummaryAppService(
            IRepository<AttendanceDetail> attendanceRepository,
            IRepository<Employees> employeesRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeesRepository = employeesRepository;
        }
        public List<AttendanceSummaryDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short AttendanceYear, short AttendanceMonth)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var employeeAttendance = _attendanceRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.AttendanceDate.Value.Month == AttendanceMonth && o.AttendanceDate.Value.Year == AttendanceYear);

            var employees = _employeesRepository.GetAll().Where(o => o.TenantId == TenantId && o.Active == true &&
            o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
            o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
            o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));


            var data = from o in employeeAttendance
                       join b in employees on o.EmployeeID equals b.EmployeeID
                       orderby o.AttendanceDate
                       select new AttendanceSummaryDto()
                       {
                           EmployeeID = o.EmployeeID.ToString(),
                           EmployeeName = b.EmployeeName,
                           AttendanceDate = o.AttendanceDate == null ? o.AttendanceDate.ToString() : o.AttendanceDate.Value.ToString("dd/MM/yyyy"),
                           Attendance = o.TotalHrs >= 8 ? "Present" : o.LeaveType == "W" ? "Rest" : "Absent",
                           MonthYear = o.AttendanceDate.Value.ToString("MMMM/yyyy")
                       }; return data.ToList();


        }


    }
}

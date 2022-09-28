using Abp.Authorization;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.PayRoll.Attendance;
using ERP.PayRoll.Designation;
using ERP.PayRoll.Employees;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_AttendanceReports)]
    public class AttendanceTransactionAppService : ERPReportAppServiceBase, IAttendanceTransactionAppService
    {
        private readonly IRepository<AttendanceDetail> _attendanceRepository;
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IRepository<Designations> _designationRepository;


        public AttendanceTransactionAppService(IRepository<AttendanceDetail> attendanceRepository,
            IRepository<Employees> employeesRepository, IRepository<Designations> designationRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeesRepository = employeesRepository;
            _designationRepository = designationRepository;
        }
        public List<AttendanceTransactionDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short AttendanceYear, short AttendanceMonth)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var employeeAttendance = _attendanceRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.AttendanceDate.Value.Month == AttendanceMonth && o.AttendanceDate.Value.Year == AttendanceYear);

            var employeesData = _employeesRepository.GetAll().Where(o => o.TenantId == TenantId && o.Active == true &&
            o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
            o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
            o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));
            var designationData = _designationRepository.GetAll().Where(o => o.TenantId == TenantId);


            var data = from a in employeeAttendance
                       join b in employeesData on a.EmployeeID equals b.EmployeeID
                       join c in designationData on b.DesignationID equals c.DesignationID

                       orderby a.AttendanceDate
                       select new AttendanceTransactionDto()
                       {
                           EmployeeID = a.EmployeeID.ToString() + " - " + b.EmployeeName + " (" + c.Designation + ")",
                           AttendanceDate = a.AttendanceDate.Value.ToString("dd-MMM-yyyy"),
                           AttendanceDay = a.AttendanceDate.Value.ToString("dddd"),
                           TimeIn = a.TimeIn == null ? "" : a.TimeIn.Value.ToString("hh:mm tt"),
                           TimeOut = a.TimeOut == null ? "" : a.TimeOut.Value.ToString("hh:mm tt"),
                           BreakOut = a.BreakOut == null ? "" : a.BreakOut.Value.ToString("hh:mm tt"),
                           BreakIn = a.BreakIn == null ? "" : a.BreakIn.Value.ToString("hh:mm tt"),
                           TotalHrs = a.TotalHrs.ToString(),
                           LeaveType = a.LeaveType,
                           Reason = a.Reason,
                           MonthYear = a.AttendanceDate.Value.ToString("MMMM yyyy")
                       };

            return data.ToList();
        }


    }
}

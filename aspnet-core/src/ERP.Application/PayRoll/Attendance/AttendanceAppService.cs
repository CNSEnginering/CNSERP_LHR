using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Attendance.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using AutoMapper;

namespace ERP.PayRoll.Attendance
{
    [AbpAuthorize(AppPermissions.PayRoll_Attendance)]
    public class AttendanceAppService : ERPAppServiceBase, IAttendanceAppService
    {
        private readonly IRepository<AttendanceDetail> _attendanceRepository;
        private readonly IRepository<Employees.Employees> _employeesRepository;
        private readonly IRepository<Designation.Designations> _designationRepository;
        private readonly IRepository<Shifts.Shift> _shiftRepository;

        public AttendanceAppService(IRepository<AttendanceDetail> attendanceRepository, IRepository<Employees.Employees> employeesRepository,
            IRepository<Designation.Designations> designationRepository, IRepository<Shifts.Shift> shiftRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeesRepository = employeesRepository;
            _designationRepository = designationRepository;
            _shiftRepository = shiftRepository;
        }

        public async Task<string> UpdateAttendance(CreateOrEditAttendanceDetailDto input)
        {
            AttendanceDetail attendanceObj;
            try
            {
                attendanceObj = _attendanceRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.EmployeeID == input.EmployeeID &&
            e.AttendanceDate.Value.ToString("MM/dd/yyyy") == input.AttendanceDate.Value.ToString("MM/dd/yyyy")).FirstOrDefault();

                if (attendanceObj == null)
                    throw new Exception("No Attendance");

            }
            catch (Exception)
            {
                return "failed";
            }

            var att = ObjectMapper.Map<CreateOrEditAttendanceDetailDto>(attendanceObj);

            att.TimeIn = input.TimeIn;
            att.TimeOut = input.TimeOut;
            att.Reason = input.Reason;

            double? totalHours = 0;
            if (input.TimeIn != null && input.TimeOut != null)
            {
                totalHours = (input.TimeOut - input.TimeIn).Value.TotalHours;
            }
            totalHours = (totalHours ?? 0);
            totalHours = Math.Round((double)totalHours, 2);
            att.TotalHrs = Math.Abs((decimal)totalHours);

            var attendance = await _attendanceRepository.FirstOrDefaultAsync((int)att.Id);
            ObjectMapper.Map(att, attendance);
            return "done";
        }

        public EmployeeDataForAttendanceDto EmployeeDataForAttendance(int employeeID, DateTime attendanceDate)
        {
            AttendanceDetail attendanceObj;
            var data = new EmployeeDataForAttendanceDto();

            try
            {
                attendanceObj = _attendanceRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.EmployeeID == employeeID &&
           e.AttendanceDate.Value.ToString("MM/dd/yyyy") == attendanceDate.ToString("MM/dd/yyyy")).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return data;
            }

            var employee = _employeesRepository.GetAll().Where(e => e.EmployeeID == employeeID && e.Active == true && e.TenantId == AbpSession.TenantId).FirstOrDefault();
            var designationID = employee.DesignationID;
            var shiftID = employee.ShiftID;

            var designation = _designationRepository.GetAll().Where(e => e.DesignationID == designationID && e.TenantId == AbpSession.TenantId).FirstOrDefault(); ;
            var shift = _shiftRepository.GetAll().Where(e => e.ShiftID == shiftID && e.TenantId == AbpSession.TenantId).FirstOrDefault();


            data.DesignationID = designation.DesignationID;
            data.Designation = designation.Designation;
            data.ShiftID = shift.ShiftID;
            data.ShiftName = shift.ShiftName;
            if (attendanceObj != null)
            {
                if (attendanceObj.TimeIn != null)
                {
                    data.TimeIn = attendanceObj.TimeIn;
                }
                if (attendanceObj.TimeOut != null)
                {
                    data.TimeOut = attendanceObj.TimeOut;
                }
                data.Reason = attendanceObj.Reason;
            }


            return data;
        }

        public string ScheduleAttendance(DateTime fromDate, DateTime toDate)
        {
            int TenantId = (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("sp_createschedule", cn))
                {
                    cmd.CommandTimeout = 2500;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromdate", fromDate);
                    cmd.Parameters.AddWithValue("@todate", toDate);
                    cmd.Parameters.AddWithValue("@TenantID", TenantId);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    // // cn.Close();
                }
            }
            return "done";
        }

        public string MarkWholeMonthAttendance(DateTime fromDate, DateTime toDate)
        {
            int TenantId = (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateAttendance_FM", cn))
                {
                    cmd.CommandTimeout = 2500;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromdate", fromDate);
                    cmd.Parameters.AddWithValue("@todate", toDate);
                    cmd.Parameters.AddWithValue("@TenantID", TenantId);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    // // cn.Close();
                }
                using (SqlCommand cmd = new SqlCommand("sp_updatepayroll", cn))
                {
                    cmd.CommandTimeout = 2500;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromdate", fromDate);
                    cmd.Parameters.AddWithValue("@todate", toDate);
                    cmd.Parameters.AddWithValue("@TenantID", TenantId);
                    //cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return "done";
        }




        public async Task<PagedResultDto<AttendanceDetailDto>> GetAttendanceData(DateTime attendanceDate)
        {
            var attendanceData = _attendanceRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId
                                 && e.AttendanceDate.Value.ToString("MM/dd/yyyy") == attendanceDate.ToString("MM/dd/yyyy"));

            var employees = _employeesRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Active == true);
            var shifts = _shiftRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId);

            var attendanceDetails = from a in attendanceData
                                    join b in shifts on a.ShiftID equals b.ShiftID
                                    join c in employees on a.EmployeeID equals c.EmployeeID
                                    select new AttendanceDetailDto
                                    {
                                        EmployeeID = a.EmployeeID,
                                        EmployeeName = c.EmployeeName,
                                        ShiftID = a.ShiftID,
                                        TimeIn = a.TimeIn == null ? b.StartTime : a.TimeIn,
                                        TimeOut = a.TimeOut == null ? b.EndTime : a.TimeOut,
                                        Id = a.Id
                                    };

            var totalCount = await attendanceDetails.CountAsync();

            return new PagedResultDto<AttendanceDetailDto>(
                totalCount,
                await attendanceDetails.ToListAsync()
            );

        }
        public async Task<PagedResultDto<AttendanceDetailDto>> GetEmployeeAttendance(DateTime attendanceDate, int empId)
        {
            var attendanceData = _attendanceRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId
                                 && e.AttendanceDate.Value.Month == attendanceDate.Month && e.AttendanceDate.Value.Year == attendanceDate.Year && e.EmployeeID == empId);


            var attendanceDetails = from a in attendanceData
                                    join c in _employeesRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on
                                    new { a.TenantId, a.EmployeeID } equals new { c.TenantId, c.EmployeeID }
                                    select new AttendanceDetailDto
                                    {
                                        EmployeeID = a.EmployeeID,
                                        EmployeeName = c.EmployeeName,
                                        AttendanceDate = a.AttendanceDate,
                                        include = false,
                                        Id = a.Id
                                    };

            var totalCount = await attendanceDetails.CountAsync();

            return new PagedResultDto<AttendanceDetailDto>(
                totalCount,
                await attendanceDetails.ToListAsync()
            );
        }
        public async Task UpdateBulkAttendanceByEmp(IEnumerable<AttendanceDetailDto> input)
        {


            var attendanceToUpdate = input.Where(x => x.include == true);

            var attendance = from a in await _attendanceRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToListAsync()
                             join b in attendanceToUpdate on new { a.EmployeeID, a.AttendanceDate } equals new { b.EmployeeID, b.AttendanceDate }
                             select a;


            foreach (var item in attendance)
            {

                item.TimeIn = null;
                item.TimeOut = null;
                item.TotalHrs = null;
                item.LeaveType = null;
                item.LeaveHours = null;
                item.GraceHours = null;

                await _attendanceRepository.UpdateAsync(item);

            }

        }   

        public async Task UpdateBulkAttendance(CreateOrEditAttendanceHeaderDto input)
        {
            double? totalHours;
            foreach (var item in input.AttendanceDetail)
            {
                var attendance = await _attendanceRepository.FirstOrDefaultAsync((int)item.Id);
                item.AttendanceDate = attendance.AttendanceDate;
                item.TenantId = attendance.TenantId;

                totalHours = 0;
                if (item.TimeIn != null || item.TimeOut != null)
                {
                    totalHours = ((DateTime)item.TimeOut - (DateTime)item.TimeIn).TotalHours;
                }
                totalHours = (totalHours ?? 0);
                totalHours = Math.Round((double)totalHours, 2);
                item.TotalHrs = Convert.ToDecimal(totalHours);

                ObjectMapper.Map(item, attendance);
            }
        }

    }
}

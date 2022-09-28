using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Attendance.Exporting;
using ERP.PayRoll.Attendance.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.PayRoll.AttendanceDetail;
using Abp.Collections.Extensions;
using System.Collections.Generic;
using System;

namespace ERP.PayRoll.Attendance
{
    [AbpAuthorize(AppPermissions.PayRoll_AttendanceDetail)]
    public class AttendanceDetailAppService : ERPAppServiceBase, IAttendanceDetailAppService
    {
        private readonly IRepository<AttendanceDetail> _attendanceDetailRepository;
        private readonly IRepository<Employees.Employees> _employeeRepository;
        private readonly IRepository<Shifts.Shift> _shiftRepository;
        private readonly IAttendanceDetailExcelExporter _attendanceDetailExcelExporter;


        public AttendanceDetailAppService(
            IRepository<AttendanceDetail> attendanceDetailRepository,
            IRepository<Employees.Employees> employeeRepository,
            IRepository<Shifts.Shift> shiftRepository,
            IAttendanceDetailExcelExporter attendanceDetailExcelExporter)
        {
            _attendanceDetailRepository = attendanceDetailRepository;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
            _attendanceDetailExcelExporter = attendanceDetailExcelExporter;

        }

        public async Task<PagedResultDto<GetAttendanceDetailForViewDto>> GetAll(GetAllAttendanceDetailInput input)
        {

            var filteredAttendanceDetail = _attendanceDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmployeeName.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinAttendanceDateFilter != null, e => e.AttendanceDate >= input.MinAttendanceDateFilter)
                        .WhereIf(input.MaxAttendanceDateFilter != null, e => e.AttendanceDate <= input.MaxAttendanceDateFilter)
                        .WhereIf(input.MinShiftIDFilter != null, e => e.ShiftID >= input.MinShiftIDFilter)
                        .WhereIf(input.MaxShiftIDFilter != null, e => e.ShiftID <= input.MaxShiftIDFilter)
                        .WhereIf(input.MinTimeInFilter != null, e => e.TimeIn >= input.MinTimeInFilter)
                        .WhereIf(input.MaxTimeInFilter != null, e => e.TimeIn <= input.MaxTimeInFilter)
                        .WhereIf(input.MinTimeOutFilter != null, e => e.TimeOut >= input.MinTimeOutFilter)
                        .WhereIf(input.MaxTimeOutFilter != null, e => e.TimeOut <= input.MaxTimeOutFilter)
                        .WhereIf(input.MinBreakOutFilter != null, e => e.BreakOut >= input.MinBreakOutFilter)
                        .WhereIf(input.MaxBreakOutFilter != null, e => e.BreakOut <= input.MaxBreakOutFilter)
                        .WhereIf(input.MinBreakInFilter != null, e => e.BreakIn >= input.MinBreakInFilter)
                        .WhereIf(input.MaxBreakInFilter != null, e => e.BreakIn <= input.MaxBreakInFilter)
                        .WhereIf(input.MinTotalHrsFilter != null, e => e.TotalHrs >= input.MinTotalHrsFilter)
                        .WhereIf(input.MaxTotalHrsFilter != null, e => e.TotalHrs <= input.MaxTotalHrsFilter);

            var pagedAndFilteredAttendanceDetail = filteredAttendanceDetail
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var attendanceDetail = from o in pagedAndFilteredAttendanceDetail
                                   select new GetAttendanceDetailForViewDto()
                                   {
                                       AttendanceDetail = new AttendanceDetailDto
                                       {
                                           DetID = o.DetID,
                                           EmployeeID = o.EmployeeID,
                                           EmployeeName = o.EmployeeName,
                                           AttendanceDate = o.AttendanceDate,
                                           ShiftID = o.ShiftID,
                                           TimeIn = o.TimeIn,
                                           TimeOut = o.TimeOut,
                                           BreakOut = o.BreakOut,
                                           BreakIn = o.BreakIn,
                                           TotalHrs = o.TotalHrs,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredAttendanceDetail.CountAsync();

            return new PagedResultDto<GetAttendanceDetailForViewDto>(
                totalCount,
                await attendanceDetail.ToListAsync()
            );
        }


        [AbpAuthorize(AppPermissions.PayRoll_AttendanceDetail_Edit)]
        public async Task<GetAttendanceDetailForEditOutput> GetAttendanceDetailForEdit(int ID)
        {
            var attendanceDetail = await _attendanceDetailRepository.GetAllListAsync(x => x.DetID == ID && x.TenantId == AbpSession.TenantId);

            var attendanceDetails = from o in attendanceDetail
                                    select new CreateOrEditAttendanceDetailDto
                                    {
                                        DetID = o.DetID,
                                        EmployeeID = o.EmployeeID,
                                        EmployeeName = o.EmployeeName,
                                        AttendanceDate = o.AttendanceDate,
                                        ShiftID = o.ShiftID,
                                        TimeIn = o.TimeIn,
                                        TimeOut = o.TimeOut,
                                        BreakOut = o.BreakOut,
                                        BreakIn = o.BreakIn,
                                        TotalHrs = o.TotalHrs,
                                        Id = o.Id

                                    };


            var output = new GetAttendanceDetailForEditOutput { AttendanceDetail = ObjectMapper.Map<ICollection<CreateOrEditAttendanceDetailDto>>(attendanceDetails) };

            return output;
        }

        public async Task CreateOrEdit(ICollection<CreateOrEditAttendanceDetailDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    await Create(item);
                }
                else
                {
                    await Update(item);
                }
            }
        }

        [AbpAuthorize(AppPermissions.PayRoll_AttendanceDetail_Create)]
        protected virtual async Task Create(CreateOrEditAttendanceDetailDto input)
        {
            AttendanceDetail attendanceDetail = new AttendanceDetail();
            attendanceDetail.DetID = (int)input.DetID;
            attendanceDetail.EmployeeID = input.EmployeeID;
            attendanceDetail.EmployeeName = input.EmployeeName;
            attendanceDetail.AttendanceDate = input.AttendanceDate;
            attendanceDetail.ShiftID = input.ShiftID;
            attendanceDetail.TimeIn = input.TimeIn;
            attendanceDetail.TimeOut = input.TimeOut;
            attendanceDetail.BreakOut = input.BreakOut;
            attendanceDetail.BreakIn = input.BreakIn;
            attendanceDetail.TotalHrs = Math.Abs((decimal)input.TotalHrs);

            //var attendanceDetail = ObjectMapper.Map<AttendanceDetail>(input);

            if (AbpSession.TenantId != null)
            {
                attendanceDetail.TenantId = (int)AbpSession.TenantId;
            }


            await _attendanceDetailRepository.InsertAsync(attendanceDetail);
        }

        [AbpAuthorize(AppPermissions.PayRoll_AttendanceDetail_Edit)]
        protected virtual async Task Update(CreateOrEditAttendanceDetailDto input)
        {
            AttendanceDetail attendanceDetail = new AttendanceDetail();
            attendanceDetail.DetID = (int)input.DetID;
            attendanceDetail.EmployeeID = input.EmployeeID;
            attendanceDetail.EmployeeName = input.EmployeeName;
            attendanceDetail.AttendanceDate = input.AttendanceDate;
            attendanceDetail.ShiftID = input.ShiftID;
            attendanceDetail.TimeIn = input.TimeIn;
            attendanceDetail.TimeOut = input.TimeOut;
            attendanceDetail.BreakOut = input.BreakOut;
            attendanceDetail.BreakIn = input.BreakIn;
            attendanceDetail.TotalHrs = Math.Abs((decimal)input.TotalHrs);

            await _attendanceDetailRepository.InsertAsync(attendanceDetail);
        }

        [AbpAuthorize(AppPermissions.PayRoll_AttendanceDetail_Delete)]
        public async Task Delete(int input)
        {
            await _attendanceDetailRepository.DeleteAsync(x => x.DetID == input);
        }

        public async Task<FileDto> GetAttendanceDetailToExcel(GetAllAttendanceDetailForExcelInput input)
        {

            var filteredAttendanceDetail = _attendanceDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmployeeName.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinAttendanceDateFilter != null, e => e.AttendanceDate >= input.MinAttendanceDateFilter)
                        .WhereIf(input.MaxAttendanceDateFilter != null, e => e.AttendanceDate <= input.MaxAttendanceDateFilter)
                        .WhereIf(input.MinShiftIDFilter != null, e => e.ShiftID >= input.MinShiftIDFilter)
                        .WhereIf(input.MaxShiftIDFilter != null, e => e.ShiftID <= input.MaxShiftIDFilter)
                        .WhereIf(input.MinTimeInFilter != null, e => e.TimeIn >= input.MinTimeInFilter)
                        .WhereIf(input.MaxTimeInFilter != null, e => e.TimeIn <= input.MaxTimeInFilter)
                        .WhereIf(input.MinTimeOutFilter != null, e => e.TimeOut >= input.MinTimeOutFilter)
                        .WhereIf(input.MaxTimeOutFilter != null, e => e.TimeOut <= input.MaxTimeOutFilter)
                        .WhereIf(input.MinBreakOutFilter != null, e => e.BreakOut >= input.MinBreakOutFilter)
                        .WhereIf(input.MaxBreakOutFilter != null, e => e.BreakOut <= input.MaxBreakOutFilter)
                        .WhereIf(input.MinBreakInFilter != null, e => e.BreakIn >= input.MinBreakInFilter)
                        .WhereIf(input.MaxBreakInFilter != null, e => e.BreakIn <= input.MaxBreakInFilter)
                        .WhereIf(input.MinTotalHrsFilter != null, e => e.TotalHrs >= input.MinTotalHrsFilter)
                        .WhereIf(input.MaxTotalHrsFilter != null, e => e.TotalHrs <= input.MaxTotalHrsFilter);

            var query = (from o in filteredAttendanceDetail
                         select new GetAttendanceDetailForViewDto()
                         {
                             AttendanceDetail = new AttendanceDetailDto
                             {
                                 EmployeeID = o.EmployeeID,
                                 EmployeeName = o.EmployeeName,
                                 AttendanceDate = o.AttendanceDate,
                                 ShiftID = o.ShiftID,
                                 TimeIn = o.TimeIn,
                                 TimeOut = o.TimeOut,
                                 BreakOut = o.BreakOut,
                                 BreakIn = o.BreakIn,
                                 TotalHrs = o.TotalHrs,
                                 Id = o.Id
                             }
                         });


            var attendanceDetailListDtos = await query.ToListAsync();

            return _attendanceDetailExcelExporter.ExportToFile(attendanceDetailListDtos);
        }

        public async Task<FileDto> GetAttendanceInExcelFile(int Id)
        {
            var attendanceDetail = await _attendanceDetailRepository.GetAllListAsync(x => x.DetID == Id && x.TenantId == AbpSession.TenantId);

            var attendanceDetails = (from o in attendanceDetail
                                     select new GetAttendanceDetailForViewDto()
                                     {
                                         AttendanceDetail = new AttendanceDetailDto
                                         {
                                             //DetID = o.DetID,
                                             EmployeeID = o.EmployeeID,
                                             EmployeeName = o.EmployeeName,
                                             //AttendanceDate = o.AttendanceDate,
                                             ShiftID = o.ShiftID,
                                             TimeIn = o.TimeIn,
                                             TimeOut = o.TimeOut,
                                             BreakOut = o.BreakOut,
                                             BreakIn = o.BreakIn,
                                             //TotalHrs = o.TotalHrs,
                                             //Id = o.Id

                                         }
                                     }).ToList();

            return _attendanceDetailExcelExporter.ExportToFile(attendanceDetails);
        }

        public async Task<GetAttendanceDetailForViewDto> GetAttendanceDetailForView(int id)
        {
            var attendanceDetail = await _attendanceDetailRepository.GetAsync(id);

            var output = new GetAttendanceDetailForViewDto { AttendanceDetail = ObjectMapper.Map<AttendanceDetailDto>(attendanceDetail) };

            return output;
        }

        public async Task<PagedResultDto<AttendanceDetailDto>> GetEmployeesData()
        {
            var employees = _employeeRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Active == true);
            var shifts = _shiftRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId);

            var attendanceDetails = from a in employees
                                    join b in shifts on a.ShiftID equals b.ShiftID
                                    select new AttendanceDetailDto
                                    {
                                        EmployeeID = a.EmployeeID,
                                        EmployeeName = a.EmployeeName,
                                        ShiftID = a.ShiftID,
                                        TimeIn = b.StartTime,
                                        TimeOut = b.EndTime

                                    };

            var totalCount = await attendanceDetails.CountAsync();

            return new PagedResultDto<AttendanceDetailDto>(
                totalCount,
                await attendanceDetails.ToListAsync()
            );

        }


    }
}
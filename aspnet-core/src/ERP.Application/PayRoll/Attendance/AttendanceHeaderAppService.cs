

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Payroll.Attendance.Exporting;
using ERP.PayRoll.AttendanceDetail;
using ERP.PayRoll.Attendance.Dtos;

namespace ERP.PayRoll.Attendance
{
    [AbpAuthorize(AppPermissions.PayRoll_AttendanceHeader)]
    public class AttendanceHeaderAppService : ERPAppServiceBase, IAttendanceHeaderAppService
    {
        private readonly IRepository<AttendanceHeader> _attendanceHeaderRepository;
        private readonly IAttendanceHeaderExcelExporter _attendanceHeadersExcelExporter;
        private readonly IAttendanceDetailAppService _attendanceDetailAppService;




        public AttendanceHeaderAppService(IRepository<AttendanceHeader> AttendanceHeaderRepository, IAttendanceHeaderExcelExporter AttendanceHeadersExcelExporter, IAttendanceDetailAppService AttendanceHeaderDetailsAppService)
        {
            _attendanceHeaderRepository = AttendanceHeaderRepository;
            _attendanceHeadersExcelExporter = AttendanceHeadersExcelExporter;
            _attendanceDetailAppService = AttendanceHeaderDetailsAppService;

        }

        public async Task<PagedResultDto<GetAttendanceHeaderForViewDto>> GetAll(GetAllAttendanceHeaderInput input)
        {

            var filteredAttendanceHeaders = _attendanceHeaderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredAttendanceHeaders = filteredAttendanceHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var AttendanceHeaders = from o in pagedAndFilteredAttendanceHeaders
                                    select new GetAttendanceHeaderForViewDto()
                                    {
                                        AttendanceHeader = new AttendanceHeaderDto
                                        {
                                            DocDate = o.DocDate,
                                            CreatedBy = o.CreatedBy,
                                            CreateDate = o.CreateDate,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredAttendanceHeaders.CountAsync();

            return new PagedResultDto<GetAttendanceHeaderForViewDto>(
                totalCount,
                await AttendanceHeaders.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.PayRoll_AttendanceHeader_Edit)]
        public async Task<GetAttendanceHeaderForEditOutput> GetAttendanceHeaderForEdit(EntityDto input)
        {
            var attendanceHeader = await _attendanceHeaderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAttendanceHeaderForEditOutput { AttendanceHeader = ObjectMapper.Map<CreateOrEditAttendanceHeaderDto>(attendanceHeader) };

            var attendanceDetail = await _attendanceDetailAppService.GetAttendanceDetailForEdit((int)output.AttendanceHeader.Id);
            output.AttendanceHeader.AttendanceDetail = attendanceDetail.AttendanceDetail;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAttendanceHeaderDto input)
        {

            if (!input.flag)
            {
                await Create(input);


            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.PayRoll_AttendanceHeader_Create)]
        protected virtual async Task Create(CreateOrEditAttendanceHeaderDto input)
        {
            var attendanceHeader = ObjectMapper.Map<AttendanceHeader>(input);


            if (AbpSession.TenantId != null)
            {
                attendanceHeader.TenantId = (int)AbpSession.TenantId;
            }


            var Hid = await _attendanceHeaderRepository.InsertAndGetIdAsync(attendanceHeader);

            double? totalHours;
            foreach (var item in input.AttendanceDetail)
            {
                item.DetID = Hid;


                totalHours = 0;
                if (item.TimeIn != null || item.TimeOut != null)
                {
                    totalHours = ((DateTime)item.TimeOut - (DateTime)item.TimeIn).TotalHours;
                }
                totalHours = (totalHours ?? 0);
                totalHours = Math.Round((double)totalHours, 2);
                item.TotalHrs = Convert.ToDecimal(totalHours);
            }

            await _attendanceDetailAppService.CreateOrEdit(input.AttendanceDetail);
        }

        [AbpAuthorize(AppPermissions.PayRoll_AttendanceHeader_Edit)]
        protected virtual async Task Update(CreateOrEditAttendanceHeaderDto input)
        {
            var attendanceHeader = await _attendanceHeaderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, attendanceHeader);

            foreach (var item in input.AttendanceDetail)
            {
                item.DetID = (int)input.Id;
            }

            await _attendanceDetailAppService.Delete((int)input.Id);
            await _attendanceDetailAppService.CreateOrEdit(input.AttendanceDetail);
        }

        [AbpAuthorize(AppPermissions.PayRoll_AttendanceHeader_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _attendanceHeaderRepository.DeleteAsync(input.Id);
            await _attendanceDetailAppService.Delete(input.Id);
        }

        public async Task<FileDto> GetAttendanceHeaderToExcel(GetAllAttendanceHeaderForExcelInput input)
        {

            var filteredAttendanceHeaders = _attendanceHeaderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredAttendanceHeaders
                         select new GetAttendanceHeaderForViewDto()
                         {
                             AttendanceHeader = new AttendanceHeaderDto
                             {
                                 DocDate = o.DocDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var attendanceHeaderListDtos = await query.ToListAsync();

            return _attendanceHeadersExcelExporter.ExportToFile(attendanceHeaderListDtos);
        }

    }
}
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using ERP.PayRoll.Shifts.Dtos;
using ERP.PayRoll.Shift.Exporting;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ERP.PayRoll.Shifts
{
    [AbpAuthorize(AppPermissions.PayRoll_Shifts)]
    public class ShiftAppService : ERPAppServiceBase, IShiftAppService
    {
        private readonly IRepository<Shift> _shiftRepository;
        private readonly IShiftExcelExporter _shiftExcelExporter;

        public ShiftAppService(IRepository<Shift> shiftRepository, IShiftExcelExporter shiftExcelExporter)
        {
            _shiftRepository = shiftRepository;
            _shiftExcelExporter = shiftExcelExporter;
        }

        public async Task<PagedResultDto<GetShiftForViewDto>> GetAll(GetAllShiftInput input)
        {
            var filteredShifts = _shiftRepository.GetAll()
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ShiftName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                .WhereIf(input.MinShiftIDFilter != null, e => e.ShiftID >= input.MinShiftIDFilter)
                                .WhereIf(input.MaxShiftIDFilter != null, e => e.ShiftID <= input.MaxShiftIDFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameFilter), e => e.ShiftName.ToLower() == input.ShiftNameFilter.ToLower().Trim())
                                .WhereIf(input.MinStartTimeFilter != null, e => e.StartTime >= input.MinStartTimeFilter)
                                .WhereIf(input.MaxStartTimeFilter != null, e => e.StartTime <= input.MaxStartTimeFilter)
                                .WhereIf(input.MinEndTimeFilter != null, e => e.EndTime >= input.MinEndTimeFilter)
                                .WhereIf(input.MaxEndTimeFilter != null, e => e.EndTime <= input.MaxEndTimeFilter)
                                .WhereIf(input.MinBeforeStartFilter != null, e => e.BeforeStart >= input.MinBeforeStartFilter)
                                .WhereIf(input.MaxBeforeStartFilter != null, e => e.BeforeStart <= input.MaxBeforeStartFilter)
                                .WhereIf(input.MinAfterStartFilter != null, e => e.AfterStart >= input.MinAfterStartFilter)
                                .WhereIf(input.MaxAfterStartFilter != null, e => e.AfterStart <= input.MaxAfterStartFilter)
                                .WhereIf(input.MinBeforeFinishFilter != null, e => e.BeforeFinish >= input.MinBeforeFinishFilter)
                                .WhereIf(input.MaxBeforeFinishFilter != null, e => e.BeforeFinish <= input.MaxBeforeFinishFilter)
                                .WhereIf(input.MinAfterFinishFilter != null, e => e.AfterFinish >= input.MinAfterFinishFilter)
                                .WhereIf(input.MaxAfterFinishFilter != null, e => e.AfterFinish <= input.MaxAfterFinishFilter)
                                .WhereIf(input.MinTotalHourFilter != null, e => e.TotalHour >= input.MinTotalHourFilter)
                                .WhereIf(input.MaxTotalHourFilter != null, e => e.TotalHour <= input.MaxTotalHourFilter)
                                .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredShifts = filteredShifts
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            var shift = from o in pagedAndFilteredShifts
                        select new GetShiftForViewDto()
                        {
                            Shift = new ShiftDto
                            {
                                ShiftID = o.ShiftID,
                                ShiftName = o.ShiftName,
                                StartTime = o.StartTime,
                                EndTime = o.EndTime,
                                BeforeStart = o.BeforeStart,
                                AfterStart = o.AfterStart,
                                BeforeFinish = o.BeforeFinish,
                                AfterFinish = o.AfterFinish,
                                TotalHour = o.TotalHour,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
                            }
                        };
            var totalCount = await filteredShifts.CountAsync();

            return new PagedResultDto<GetShiftForViewDto>(
                totalCount,
                await shift.ToListAsync()
            );

        }

        public async Task<GetShiftForViewDto> GetShiftForView(int id)
        {
            var shift = await _shiftRepository.GetAsync(id);

            var output = new GetShiftForViewDto { Shift = ObjectMapper.Map<ShiftDto>(shift) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_Shifts_Edit)]
        public async Task<GetShiftForEditOutput> GetShiftForEdit(EntityDto input)
        {
            var shift = await _shiftRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetShiftForEditOutput { Shift = ObjectMapper.Map<CreateOrEditShiftDto>(shift) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditShiftDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.PayRoll_Shifts_Create)]
        protected virtual async Task Create(CreateOrEditShiftDto input)
        {

            var shift = ObjectMapper.Map<Shift>(input);


            if (AbpSession.TenantId != null)
            {
                shift.TenantId = (int)AbpSession.TenantId;
            }

            shift.ShiftID = GetMaxID();
            await _shiftRepository.InsertAsync(shift);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Shifts_Edit)]
        protected virtual async Task Update(CreateOrEditShiftDto input)
        {
            var shift = await _shiftRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, shift);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Shifts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _shiftRepository.DeleteAsync(input.Id);
        }



        public async Task<FileDto> GetShiftToExcel(GetAllShiftForExcelInput input)
        {
            var filteredShifts = _shiftRepository.GetAll()
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ShiftName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                 .WhereIf(input.MinShiftIDFilter != null, e => e.ShiftID >= input.MinShiftIDFilter)
                                 .WhereIf(input.MaxShiftIDFilter != null, e => e.ShiftID <= input.MaxShiftIDFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameFilter), e => e.ShiftName.ToLower() == input.ShiftNameFilter.ToLower().Trim())
                                 .WhereIf(input.MinStartTimeFilter != null, e => e.StartTime >= input.MinStartTimeFilter)
                                 .WhereIf(input.MaxStartTimeFilter != null, e => e.StartTime <= input.MaxStartTimeFilter)
                                 .WhereIf(input.MinEndTimeFilter != null, e => e.EndTime >= input.MinEndTimeFilter)
                                 .WhereIf(input.MaxEndTimeFilter != null, e => e.EndTime <= input.MaxEndTimeFilter)
                                 .WhereIf(input.MinBeforeStartFilter != null, e => e.BeforeStart >= input.MinBeforeStartFilter)
                                 .WhereIf(input.MaxBeforeStartFilter != null, e => e.BeforeStart <= input.MaxBeforeStartFilter)
                                 .WhereIf(input.MinAfterStartFilter != null, e => e.AfterStart >= input.MinAfterStartFilter)
                                 .WhereIf(input.MaxAfterStartFilter != null, e => e.AfterStart <= input.MaxAfterStartFilter)
                                 .WhereIf(input.MinBeforeFinishFilter != null, e => e.BeforeFinish >= input.MinBeforeFinishFilter)
                                 .WhereIf(input.MaxBeforeFinishFilter != null, e => e.BeforeFinish <= input.MaxBeforeFinishFilter)
                                 .WhereIf(input.MinAfterFinishFilter != null, e => e.AfterFinish >= input.MinAfterFinishFilter)
                                 .WhereIf(input.MaxAfterFinishFilter != null, e => e.AfterFinish <= input.MaxAfterFinishFilter)
                                 .WhereIf(input.MinTotalHourFilter != null, e => e.TotalHour >= input.MinTotalHourFilter)
                                 .WhereIf(input.MaxTotalHourFilter != null, e => e.TotalHour <= input.MaxTotalHourFilter)
                                 .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                 .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                 .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                 .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                 .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);


            var query = (from o in filteredShifts
                         select new GetShiftForViewDto()
                         {
                             Shift = new ShiftDto
                             {
                                 ShiftID = o.ShiftID,
                                 ShiftName = o.ShiftName,
                                 StartTime = o.StartTime,
                                 EndTime = o.EndTime,
                                 BeforeStart = o.BeforeStart,
                                 AfterStart = o.AfterStart,
                                 BeforeFinish = o.BeforeFinish,
                                 AfterFinish = o.AfterFinish,
                                 TotalHour = o.TotalHour,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var shiftListDtos = await query.ToListAsync();

            return _shiftExcelExporter.ExportToFile(shiftListDtos);

        }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _shiftRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.ShiftID).Max() ?? 0) + 1;
            return maxid;
        }
        public double getToatalHours(string startTime, string endTime)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var totalHour = new object();
            using (SqlConnection cn = new SqlConnection(str))
            {

                DateTime sTime = Convert.ToDateTime(startTime);
                DateTime eTime = Convert.ToDateTime(endTime);

                using (SqlCommand cmd = new SqlCommand("SELECT (ISNULL(dbo.minutesToTimeF(DATEDIFF(n, @startTime, @endTime)), 0))", cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@startTime", sTime.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@endTime", eTime.ToString("HH:mm:ss"));
                    cn.Open();
                    totalHour = cmd.ExecuteScalar();
                 //   // cn.Close();
                }
            }
            return Convert.ToDouble(totalHour);
        }
    }
}

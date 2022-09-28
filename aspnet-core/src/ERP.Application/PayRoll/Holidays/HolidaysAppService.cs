

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Holidays.Exporting;
using ERP.PayRoll.Holidays.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.Holidays
{
    [AbpAuthorize(AppPermissions.PayRoll_Holidays)]
    public class HolidaysAppService : ERPAppServiceBase, IHolidaysAppService
    {
        private readonly IRepository<Holidays> _holidaysRepository;
        private readonly IHolidaysExcelExporter _holidaysExcelExporter;


        public HolidaysAppService(IRepository<Holidays> holidaysRepository, IHolidaysExcelExporter holidaysExcelExporter)
        {
            _holidaysRepository = holidaysRepository;
            _holidaysExcelExporter = holidaysExcelExporter;

        }

        public async Task<PagedResultDto<GetHolidaysForViewDto>> GetAll(GetAllHolidaysInput input)
        {

            var filteredHolidays = _holidaysRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.HolidayName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinHolidayIDFilter != null, e => e.HolidayID >= input.MinHolidayIDFilter)
                        .WhereIf(input.MaxHolidayIDFilter != null, e => e.HolidayID <= input.MaxHolidayIDFilter)
                        .WhereIf(input.MinHolidayDateFilter != null, e => e.HolidayDate >= input.MinHolidayDateFilter)
                        .WhereIf(input.MaxHolidayDateFilter != null, e => e.HolidayDate <= input.MaxHolidayDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HolidayNameFilter), e => e.HolidayName == input.HolidayNameFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredHolidays = filteredHolidays
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var holidays = from o in pagedAndFilteredHolidays
                           select new GetHolidaysForViewDto()
                           {
                               Holidays = new HolidaysDto
                               {
                                   HolidayID = o.HolidayID,
                                   HolidayDate = o.HolidayDate,
                                   HolidayName = o.HolidayName,
                                   Active = o.Active,
                                   AudtUser = o.AudtUser,
                                   AudtDate = o.AudtDate,
                                   CreatedBy = o.CreatedBy,
                                   CreateDate = o.CreateDate,
                                   Id = o.Id
                               }
                           };

            var totalCount = await filteredHolidays.CountAsync();

            return new PagedResultDto<GetHolidaysForViewDto>(
                totalCount,
                await holidays.ToListAsync()
            );
        }

        public async Task<GetHolidaysForViewDto> GetHolidaysForView(int id)
        {
            var holidays = await _holidaysRepository.GetAsync(id);

            var output = new GetHolidaysForViewDto { Holidays = ObjectMapper.Map<HolidaysDto>(holidays) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_Holidays_Edit)]
        public async Task<GetHolidaysForEditOutput> GetHolidaysForEdit(EntityDto input)
        {
            var holidays = await _holidaysRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHolidaysForEditOutput { Holidays = ObjectMapper.Map<CreateOrEditHolidaysDto>(holidays) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHolidaysDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_Holidays_Create)]
        protected virtual async Task Create(CreateOrEditHolidaysDto input)
        {
            var holidays = ObjectMapper.Map<Holidays>(input);


            if (AbpSession.TenantId != null)
            {
                holidays.TenantId = (int)AbpSession.TenantId;
            }

            holidays.HolidayID = GetMaxID();
            await _holidaysRepository.InsertAsync(holidays);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Holidays_Edit)]
        protected virtual async Task Update(CreateOrEditHolidaysDto input)
        {
            var holidays = await _holidaysRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, holidays);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Holidays_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _holidaysRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHolidaysToExcel(GetAllHolidaysForExcelInput input)
        {

            var filteredHolidays = _holidaysRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.HolidayName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinHolidayIDFilter != null, e => e.HolidayID >= input.MinHolidayIDFilter)
                        .WhereIf(input.MaxHolidayIDFilter != null, e => e.HolidayID <= input.MaxHolidayIDFilter)
                        .WhereIf(input.MinHolidayDateFilter != null, e => e.HolidayDate >= input.MinHolidayDateFilter)
                        .WhereIf(input.MaxHolidayDateFilter != null, e => e.HolidayDate <= input.MaxHolidayDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HolidayNameFilter), e => e.HolidayName == input.HolidayNameFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredHolidays
                         select new GetHolidaysForViewDto()
                         {
                             Holidays = new HolidaysDto
                             {
                                 HolidayID = o.HolidayID,
                                 HolidayDate = o.HolidayDate,
                                 HolidayName = o.HolidayName,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var holidaysListDtos = await query.ToListAsync();

            return _holidaysExcelExporter.ExportToFile(holidaysListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _holidaysRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.HolidayID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Dto;
using ERP.PayRoll.Location.Dtos;
using ERP.PayRoll.Location.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;

namespace ERP.PayRoll.Location
{
    [AbpAuthorize(AppPermissions.PayRoll_Locations)]
    public class LocationAppService : ERPAppServiceBase, ILocationAppService
    {
        private readonly IRepository<Locations> _locationRepository;
        private readonly ILocationExcelExporter _locationExcelExporter;

        public LocationAppService(IRepository<Locations> locationRepository, ILocationExcelExporter locationExcelExporter)
        {
            _locationRepository = locationRepository;
            _locationExcelExporter = locationExcelExporter;
        }

        public async Task<PagedResultDto<GetLocationForViewDto>> GetAll(GetAllLocationInput input)
        {
            var filteredLocations = _locationRepository.GetAll()
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Location.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                                .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.ToLower() == input.LocationFilter.ToLower().Trim())
                                .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredLocations = filteredLocations
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            var location = from o in pagedAndFilteredLocations
                         select new GetLocationForViewDto()
                         {
                             Location = new LocationDto
                             {
                                 LocID = o.LocID,
                                 Location = o.Location,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         };
            var totalCount = await filteredLocations.CountAsync();

            return new PagedResultDto<GetLocationForViewDto>(
                totalCount,
                await location.ToListAsync()
            );

        }

        public async Task<GetLocationForViewDto> GetLocationForView(int id)
        {
            var location = await _locationRepository.GetAsync(id);

            var output = new GetLocationForViewDto { Location = ObjectMapper.Map<LocationDto>(location) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_Locations_Edit)]
        public async Task<GetLocationForEditOutput> GetLocationForEdit(EntityDto input)
        {
            var location = await _locationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLocationForEditOutput { Location = ObjectMapper.Map<CreateOrEditLocationDto>(location) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLocationDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_Locations_Create)]
        protected virtual async Task Create(CreateOrEditLocationDto input)
        {
            var location = ObjectMapper.Map<Locations>(input);


            if (AbpSession.TenantId != null)
            {
                location.TenantId = (int)AbpSession.TenantId;
            }

            location.LocID = GetMaxID();
            await _locationRepository.InsertAsync(location);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Locations_Edit)]
        protected virtual async Task Update(CreateOrEditLocationDto input)
        {
            var location = await _locationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, location);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Locations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _locationRepository.DeleteAsync(input.Id);
        }

       
        
        public async Task<FileDto> GetLocationToExcel(GetAllLocationForExcelInput input)
        {
            var filteredLocations = _locationRepository.GetAll()
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Location.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                 .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                                 .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.LocationFilter), e => e.Location.ToLower() == input.LocationFilter.ToLower().Trim())
                                 .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                 .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                 .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                 .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                 .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);


            var query = (from o in filteredLocations
                         select new GetLocationForViewDto()
                         {
                             Location = new LocationDto
                             {
                                 LocID = o.LocID,
                                 Location = o.Location,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var locationListDtos = await query.ToListAsync();

            return _locationExcelExporter.ExportToFile(locationListDtos);

        }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.LocID).Max() ?? 0) + 1;
            return maxid;
        }
    }
}
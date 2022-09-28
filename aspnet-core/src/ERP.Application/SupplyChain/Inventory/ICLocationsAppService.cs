

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Exporting;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_ICLocations)]
    public class ICLocationsAppService : ERPAppServiceBase, IICLocationsAppService
    {
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<ICELocation> _iceLocationRepository;
        private readonly IICLocationsExcelExporter _icLocationsExcelExporter;


        public ICLocationsAppService(IRepository<ICLocation> icLocationRepository, IICLocationsExcelExporter icLocationsExcelExporter, IRepository<ICELocation> iceLocationRepository)
        {
            _icLocationRepository = icLocationRepository;
            _icLocationsExcelExporter = icLocationsExcelExporter;
            _iceLocationRepository = iceLocationRepository;
        }

        public async Task<PagedResultDto<ICLocationDto>> GetAll(GetAllICLocationsInput input)
        {

            var filteredICLocations = _icLocationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocName.Contains(input.Filter) || e.LocShort.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
                        .WhereIf(input.MinLocIDFilter != null, e => e.ParentID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.ParentID <= input.MaxLocIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocNameFilter), e => e.LocName.ToLower() == input.LocNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocShortFilter), e => e.LocShort.ToLower() == input.LocShortFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.ToLower() == input.AddressFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.ToLower() == input.CityFilter.ToLower().Trim())
                        .WhereIf(input.AllowRecFilter > -1, e => Convert.ToInt32(e.AllowRec) == input.AllowRecFilter)
                        .WhereIf(input.AllowNegFilter > -1, e => Convert.ToInt32(e.AllowNeg) == input.AllowNegFilter)
                        .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var pagedAndFilteredICLocations = filteredICLocations
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var icLocations = from o in pagedAndFilteredICLocations
                              select new ICLocationDto()
                              {
                                  LocID = o.LocID,
                                  LocName = o.LocName,
                                  LocShort = o.LocShort,
                                  Address = o.Address,
                                  City = o.City,
                                  AllowRec = o.AllowRec,
                                  AllowNeg = o.AllowNeg,
                                  IsParent = o.IsParent,
                                  ParentID = o.ParentID,
                                  Active = o.Active,
                                  CreatedBy = o.CreatedBy,
                                  CreateDate = o.CreateDate,
                                  AudtUser = o.AudtUser,
                                  AudtDate = o.AudtDate,
                                  Id = o.Id
                              };

            var totalCount = await filteredICLocations.CountAsync();

            return new PagedResultDto<ICLocationDto>(
                totalCount,
                await icLocations.ToListAsync()
            );
        }


        [AbpAuthorize(AppPermissions.Inventory_ICLocations)]
        public async Task<PagedResultDto<ICLocationDto>> GetAllICLocationForLookupTable(GetAllICLocationsInput input)
        {
            var query = _icLocationRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.LocID.ToString().Contains(input.Filter) || e.LocName.Contains(input.Filter)).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var datalist = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ICLocationDto>();
            foreach (var items in datalist)
            {
                lookupTableDtoList.Add(new ICLocationDto
                {
                    Id = items.Id,
                    LocID = items.LocID,
                    LocName = items.LocName
                });
            }

            return new PagedResultDto<ICLocationDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<ICLocationDto> GetICLocationForView(int id)
        {
            var icLocation = await _icLocationRepository.GetAsync(id);

            var output = ObjectMapper.Map<ICLocationDto>(icLocation);

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_ICLocations_Edit)]
        public async Task<ICLocationDto> GetICLocationForEdit(EntityDto input)
        {
            var icLocation = await _icLocationRepository.FirstOrDefaultAsync(input.Id);

            var output = ObjectMapper.Map<ICLocationDto>(icLocation);

            return output;
        }

        public async Task<string> CreateOrEdit(ICLocationDto input)
        {
            string status = "OK";
            if (input.Id == null)
            {
                var locID = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == input.LocID).Count();
                if (locID == 0)
                {
                    await Create(input);
                }
                else
                {
                    return status = "Present";
                }
            }
            else
            {
                await Update(input);
            }
            return status;
        }

        [AbpAuthorize(AppPermissions.Inventory_ICLocations_Create)]
        protected virtual async Task Create(ICLocationDto input)
        {
            var icLocation = ObjectMapper.Map<ICLocation>(input);


            if (AbpSession.TenantId != null)
            {
                icLocation.TenantId = (int)AbpSession.TenantId;
            }

            await _icLocationRepository.InsertAsync(icLocation);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICLocations_Edit)]
        protected virtual async Task Update(ICLocationDto input)
        {
            var icLocation = await _icLocationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, icLocation);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICLocations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _icLocationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetICLocationsToExcel(GetAllICLocationsInput input)
        {

            var filteredICLocations = _icLocationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocName.Contains(input.Filter) || e.LocShort.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocNameFilter), e => e.LocName.ToLower() == input.LocNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocShortFilter), e => e.LocShort.ToLower() == input.LocShortFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.ToLower() == input.AddressFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.ToLower() == input.CityFilter.ToLower().Trim())
                        .WhereIf(input.AllowRecFilter > -1, e => Convert.ToInt32(e.AllowRec) == input.AllowRecFilter)
                        .WhereIf(input.AllowNegFilter > -1, e => Convert.ToInt32(e.AllowNeg) == input.AllowNegFilter)
                        .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var query = (from o in filteredICLocations
                         select new ICLocationDto()
                         {
                             LocID = o.LocID,
                             LocName = o.LocName,
                             LocShort = o.LocShort,
                             Address = o.Address,
                             City = o.City,
                             AllowRec = o.AllowRec,
                             AllowNeg = o.AllowNeg,
                             IsParent = o.IsParent,
                             ParentID = o.ParentID,
                             Active = o.Active,
                             CreatedBy = o.CreatedBy,
                             CreateDate = o.CreateDate,
                             AudtUser = o.AudtUser,
                             AudtDate = o.AudtDate,
                             Id = o.Id
                         });


            var icLocationListDtos = await query.ToListAsync();

            return _icLocationsExcelExporter.ExportToFile(icLocationListDtos);
        }

        public int GetMaxLocId()
        {
            var maxid = ((from tab1 in _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.LocID).Max() ?? 0) + 1;
            return maxid;
        }

        public string GetName(int Id)
        {
            var LocName = _icLocationRepository.GetAll().Where(x => x.LocID == Id).Select(x => x.LocName).FirstOrDefault();
            return LocName;
        }

        public List<ICELocation> GetRegions(int parentId)
        {
            List<ICELocation> data = new List<ICELocation>();
            if (parentId == 0)
            {
                data = (from a in _iceLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ParentId == null)
                        select new ICELocation()
                        {
                            Id = a.Id,
                            LocationTitle = a.LocationTitle
                        }).ToList();
            }
            else
            {
                data = (from a in _iceLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                        where (a.ParentId == parentId)
                        select new ICELocation()
                        {
                            Id = a.Id,
                            LocationTitle = a.LocationTitle
                        }).ToList();
            }
            data.Insert(0, new ICELocation { Id = 0, LocationTitle = "Please Select" });
            return data;
        }
    }
}
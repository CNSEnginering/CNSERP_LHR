

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
	[AbpAuthorize(AppPermissions.Inventory_ICUOMs)]
    public class ICUOMsAppService : ERPAppServiceBase, IICUOMsAppService
    {
		 private readonly IRepository<ICUOM> _icuomRepository;
		 private readonly IICUOMsExcelExporter _icuoMsExcelExporter;
		 

		  public ICUOMsAppService(IRepository<ICUOM> icuomRepository, IICUOMsExcelExporter icuoMsExcelExporter ) 
		  {
			_icuomRepository = icuomRepository;
			_icuoMsExcelExporter = icuoMsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<ICUOMDto>> GetAll(GetAllICUOMsInput input)
         {

            var testqyer = _icuomRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter), e => e.Unit.Trim() == input.UnitFilter.Trim());


            var filteredICUOMs = _icuomRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Unit.Contains(input.Filter) || e.UNITDESC.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter),  e => e.Unit.Trim() == input.UnitFilter.Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNITDESCFilter),  e => e.UNITDESC.ToLower() == input.UNITDESCFilter.ToLower().Trim())
						.WhereIf(input.MinConverFilter != null, e => e.Conver >= input.MinConverFilter)
						.WhereIf(input.MaxConverFilter != null, e => e.Conver <= input.MaxConverFilter)
						.WhereIf(input.ActiveFilter > -1,  e => Convert.ToInt32(e.Active) == input.ActiveFilter )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

			var pagedAndFilteredICUOMs = filteredICUOMs
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var icuoMs = from o in pagedAndFilteredICUOMs
                         select new ICUOMDto() {
                                Unit = o.Unit,
                                UNITDESC = o.UNITDESC,
                                Conver = o.Conver,
                                Active = o.Active,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Id = o.Id
						};

            var totalCount = await filteredICUOMs.CountAsync();

            return new PagedResultDto<ICUOMDto>(
                totalCount,
                await icuoMs.ToListAsync()
            );
         }
		 
		 public async Task<ICUOMDto> GetICUOMForView(int id)
         {
            var icuom = await _icuomRepository.GetAsync(id);

            var output = ObjectMapper.Map<ICUOMDto>(icuom) ;
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_ICUOMs_Edit)]
		 public async Task<ICUOMDto> GetICUOMForEdit(EntityDto input)
         {
            var icuom = await _icuomRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<ICUOMDto>(icuom);
			
            return output;
         }

		 public async Task CreateOrEdit(ICUOMDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICUOMs_Create)]
		 protected virtual async Task Create(ICUOMDto input)
         {
            var icuom = ObjectMapper.Map<ICUOM>(input);

			
			if (AbpSession.TenantId != null)
			{
				icuom.TenantId = (int) AbpSession.TenantId;
			}
		

            await _icuomRepository.InsertAsync(icuom);
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICUOMs_Edit)]
		 protected virtual async Task Update(ICUOMDto input)
         {
            var icuom = await _icuomRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, icuom);
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICUOMs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _icuomRepository.DeleteAsync(input.Id);
         }

        [AbpAuthorize(AppPermissions.Inventory_ICUOMs)]
        public async Task<PagedResultDto<ICUOMDto>> GetAllICUOMForLookupTable(GetAllICUOMsInput input)
        {
            var query = _icuomRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter),e => e.UNITDESC.ToString().Contains(input.Filter) || e.Unit.Contains(input.Filter)).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var datalist = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ICUOMDto>();
            foreach (var items in datalist)
            {
                lookupTableDtoList.Add(new ICUOMDto
                {
                    Id = items.Id,
                    Unit=items.Unit,
                    UNITDESC=items.UNITDESC,
                    Conver=items.Conver
                });
            }

            return new PagedResultDto<ICUOMDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<FileDto> GetICUOMsToExcel(GetAllICUOMsInput input)
         {
			
			var filteredICUOMs = _icuomRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Unit.Contains(input.Filter) || e.UNITDESC.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter),  e => e.Unit.ToLower() == input.UnitFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNITDESCFilter),  e => e.UNITDESC.ToLower() == input.UNITDESCFilter.ToLower().Trim())
						.WhereIf(input.MinConverFilter != null, e => e.Conver >= input.MinConverFilter)
						.WhereIf(input.MaxConverFilter != null, e => e.Conver <= input.MaxConverFilter)
						.WhereIf(input.ActiveFilter > -1,  e => Convert.ToInt32(e.Active) == input.ActiveFilter )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

			var query = (from o in filteredICUOMs
                         select new ICUOMDto() { 
                                Unit = o.Unit,
                                UNITDESC = o.UNITDESC,
                                Conver = o.Conver,
                                Active = o.Active,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Id = o.Id
						 });


            var icuomListDtos = await query.ToListAsync();

            return _icuoMsExcelExporter.ExportToFile(icuomListDtos);
         }


        public async Task<ListResultDto<ComboboxItemDto>> GetUnitofMeasureForCombobox() {
            var query = _icuomRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Active == true);

            var totalCount = await query.CountAsync();

            var groupCategoryList = await query
                .ToListAsync();

            var lookupTableDtoList = new List<ComboboxItemDto>();
            foreach (var groupCategory in groupCategoryList)
            {
                lookupTableDtoList.Add(new ComboboxItemDto
                {

                    Value = groupCategory.Unit,
                    DisplayText = groupCategory.UNITDESC
                });
            }

            return new ListResultDto<ComboboxItemDto>(
                lookupTableDtoList
            );
        }


    }
}
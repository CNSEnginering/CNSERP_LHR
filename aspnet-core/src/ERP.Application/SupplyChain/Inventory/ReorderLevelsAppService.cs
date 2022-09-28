

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
using ERP.SupplyChain.Inventory.IC_Item;

namespace ERP.SupplyChain.Inventory
{
	[AbpAuthorize(AppPermissions.Inventory_ReorderLevels)]
    public class ReorderLevelsAppService : ERPAppServiceBase, IReorderLevelsAppService
    {
		 private readonly IRepository<ReorderLevel> _reorderLevelRepository;
		 private readonly IReorderLevelsExcelExporter _reorderLevelsExcelExporter;
        public readonly IRepository<ICItem> _ICItemRepository;

        public ReorderLevelsAppService(IRepository<ICItem> ICItemRepository, IRepository<ReorderLevel> reorderLevelRepository, IReorderLevelsExcelExporter reorderLevelsExcelExporter ) 
		  {
			_reorderLevelRepository = reorderLevelRepository;
			_reorderLevelsExcelExporter = reorderLevelsExcelExporter;
            _ICItemRepository = ICItemRepository;

          }

		 public async Task<PagedResultDto<ReorderLevelDto>> GetAll(GetAllReorderLevelsInput input)
         {
			
			var filteredReorderLevels = _reorderLevelRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.ItemId.Contains(input.Filter))
						.WhereIf(input.MinMinLevelFilter != null, e => e.MinLevel >= input.MinMinLevelFilter)
						.WhereIf(input.MaxMinLevelFilter != null, e => e.MinLevel <= input.MaxMinLevelFilter)
						.WhereIf(input.MinMaxLevelFilter != null, e => e.MaxLevel >= input.MinMaxLevelFilter)
						.WhereIf(input.MaxMaxLevelFilter != null, e => e.MaxLevel <= input.MaxMaxLevelFilter)
						.WhereIf(input.MinOrdLevelFilter != null, e => e.OrdLevel >= input.MinOrdLevelFilter)
						.WhereIf(input.MaxOrdLevelFilter != null, e => e.OrdLevel <= input.MaxOrdLevelFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(input.MinLocIdFilter != null, e => e.LocId >= input.MinLocIdFilter)
						.WhereIf(input.MaxLocIdFilter != null, e => e.LocId <= input.MaxLocIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ItemIdFilter),  e => e.ItemId == input.ItemIdFilter);

			var pagedAndFilteredReorderLevels = filteredReorderLevels
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var reorderLevels = from o in pagedAndFilteredReorderLevels
                         select new ReorderLevelDto() {
                                MinLevel = o.MinLevel,
                                MaxLevel = o.MaxLevel,
                                OrdLevel = o.OrdLevel,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                LocId = o.LocId,
                                ItemId = o.ItemId,
                                Id = o.Id
						};

            var totalCount = await filteredReorderLevels.CountAsync();

            return new PagedResultDto<ReorderLevelDto>(
                totalCount,
                await reorderLevels.ToListAsync()
            );
         }
		 
		 public async Task<ReorderLevelDto> GetReorderLevelForView(int id)
         {
            var reorderLevel = await _reorderLevelRepository.GetAsync(id);

            var output = ObjectMapper.Map<ReorderLevelDto>(reorderLevel) ;
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_ReorderLevels_Edit)]
		 public async Task<ReorderLevelDto> GetReorderLevelForEdit(EntityDto input)
         {
            var tenantid = (int)AbpSession.TenantId;
            var reorderLevel = await _reorderLevelRepository.FirstOrDefaultAsync(input.Id);
            var itemName = _ICItemRepository.GetAll().Where(x => x.ItemId == reorderLevel.ItemId && x.TenantId == tenantid).FirstOrDefault();

            var output =ObjectMapper.Map<ReorderLevelDto>(reorderLevel);
            output.ItemName = itemName.Descp;
            return output;
         }

		 public async Task CreateOrEdit(ReorderLevelDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Inventory_ReorderLevels_Create)]
		 protected virtual async Task Create(ReorderLevelDto input)
         {
            var reorderLevel = ObjectMapper.Map<ReorderLevel>(input);

			
			if (AbpSession.TenantId != null)
			{
				reorderLevel.TenantId = (int) AbpSession.TenantId;
			}
		

            await _reorderLevelRepository.InsertAsync(reorderLevel);
         }

		 [AbpAuthorize(AppPermissions.Inventory_ReorderLevels_Edit)]
		 protected virtual async Task Update(ReorderLevelDto input)
         {
            var reorderLevel = await _reorderLevelRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, reorderLevel);
         }

		 [AbpAuthorize(AppPermissions.Inventory_ReorderLevels_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _reorderLevelRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetReorderLevelsToExcel(GetAllReorderLevelsInput input)
         {
			
			var filteredReorderLevels = _reorderLevelRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.ItemId.Contains(input.Filter))
						.WhereIf(input.MinMinLevelFilter != null, e => e.MinLevel >= input.MinMinLevelFilter)
						.WhereIf(input.MaxMinLevelFilter != null, e => e.MinLevel <= input.MaxMinLevelFilter)
						.WhereIf(input.MinMaxLevelFilter != null, e => e.MaxLevel >= input.MinMaxLevelFilter)
						.WhereIf(input.MaxMaxLevelFilter != null, e => e.MaxLevel <= input.MaxMaxLevelFilter)
						.WhereIf(input.MinOrdLevelFilter != null, e => e.OrdLevel >= input.MinOrdLevelFilter)
						.WhereIf(input.MaxOrdLevelFilter != null, e => e.OrdLevel <= input.MaxOrdLevelFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(input.MinLocIdFilter != null, e => e.LocId >= input.MinLocIdFilter)
						.WhereIf(input.MaxLocIdFilter != null, e => e.LocId <= input.MaxLocIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ItemIdFilter),  e => e.ItemId == input.ItemIdFilter);

			var query = (from o in filteredReorderLevels
                         select new ReorderLevelDto() { 
                                MinLevel = o.MinLevel,
                                MaxLevel = o.MaxLevel,
                                OrdLevel = o.OrdLevel,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                LocId = o.LocId,
                                ItemId = o.ItemId,
                                Id = o.Id
						 });


            var reorderLevelListDtos = await query.ToListAsync();

            return _reorderLevelsExcelExporter.ExportToFile(reorderLevelListDtos);
         }


    }
}
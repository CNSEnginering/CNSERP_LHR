

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.ICOPT4.Exporting;
using ERP.SupplyChain.Inventory.ICOPT4.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Inventory.ICOPT4
{
	[AbpAuthorize(AppPermissions.Inventory_ICOPT4)]
    public class ICOPT4AppService : ERPAppServiceBase, IICOPT4AppService
    {
		 private readonly IRepository<ICOPT4> _icopT4Repository;
		 private readonly IICOPT4ExcelExporter _icopT4ExcelExporter;
		 

		  public ICOPT4AppService(IRepository<ICOPT4> icopT4Repository, IICOPT4ExcelExporter icopT4ExcelExporter ) 
		  {
			_icopT4Repository = icopT4Repository;
			_icopT4ExcelExporter = icopT4ExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetICOPT4ForViewDto>> GetAll(GetAllICOPT4Input input)
         {

            var filteredICOPT4 = _icopT4Repository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Descp.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinOptIDFilter != null, e => e.OptID >= input.MinOptIDFilter)
                        .WhereIf(input.MaxOptIDFilter != null, e => e.OptID <= input.MaxOptIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter), e => e.Descp == input.DescpFilter)
                        .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredICOPT4 = filteredICOPT4
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var icopT4 = from o in pagedAndFilteredICOPT4
                         select new GetICOPT4ForViewDto() {
							ICOPT4 = new ICOPT4Dto
							{
                                OptID = o.OptID,
                                Descp = o.Descp,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredICOPT4.CountAsync();

            return new PagedResultDto<GetICOPT4ForViewDto>(
                totalCount,
                await icopT4.ToListAsync()
            );
         }
		 
		 public async Task<GetICOPT4ForViewDto> GetICOPT4ForView(int id)
         {
            var icopT4 = await _icopT4Repository.GetAsync(id);

            var output = new GetICOPT4ForViewDto { ICOPT4 = ObjectMapper.Map<ICOPT4Dto>(icopT4) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_ICOPT4_Edit)]
		 public async Task<GetICOPT4ForEditOutput> GetICOPT4ForEdit(EntityDto input)
         {
            var icopT4 = await _icopT4Repository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetICOPT4ForEditOutput {ICOPT4 = ObjectMapper.Map<CreateOrEditICOPT4Dto>(icopT4)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditICOPT4Dto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICOPT4_Create)]
		 protected virtual async Task Create(CreateOrEditICOPT4Dto input)
         {
            var icopT4 = ObjectMapper.Map<ICOPT4>(input);

			
			if (AbpSession.TenantId != null)
			{
				icopT4.TenantId = (int) AbpSession.TenantId;
			}

            icopT4.OptID = GetMaxOptId();
            await _icopT4Repository.InsertAsync(icopT4);
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICOPT4_Edit)]
		 protected virtual async Task Update(CreateOrEditICOPT4Dto input)
         {
            var icopT4 = await _icopT4Repository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, icopT4);
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICOPT4_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _icopT4Repository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetICOPT4ToExcel(GetAllICOPT4ForExcelInput input)
         {
			
			var filteredICOPT4 = _icopT4Repository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Descp.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinOptIDFilter != null, e => e.OptID >= input.MinOptIDFilter)
						.WhereIf(input.MaxOptIDFilter != null, e => e.OptID <= input.MaxOptIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter),  e => e.Descp == input.DescpFilter)
                        .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var query = (from o in filteredICOPT4
                         select new GetICOPT4ForViewDto() { 
							ICOPT4 = new ICOPT4Dto
							{
                                OptID = o.OptID,
                                Descp = o.Descp,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						 });


            var icopT4ListDtos = await query.ToListAsync();

            return _icopT4ExcelExporter.ExportToFile(icopT4ListDtos);
         }

        public int GetMaxOptId()
        {
            var maxid = ((from tab1 in _icopT4Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.OptID).Max() ?? 0) + 1;
            return maxid;
        }


    }
}


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Inventory
{
	[AbpAuthorize(AppPermissions.Inventory_ICLEDG)]
    public class ICLEDGAppService : ERPAppServiceBase, IICLEDGAppService
    {
		 private readonly IRepository<ICLEDG> _icledgRepository;
		 

		  public ICLEDGAppService(IRepository<ICLEDG> icledgRepository ) 
		  {
			_icledgRepository = icledgRepository;
			
		  }

		 public async Task<PagedResultDto<GetICLEDGForViewDto>> GetAll(GetAllICLEDGInput input)
         {
			
			var filteredICLEDG = _icledgRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DocDesc.Contains(input.Filter) || e.ItemID.Contains(input.Filter) || e.srno.Contains(input.Filter) || e.UNIT.Contains(input.Filter) || e.Descp.Contains(input.Filter) || e.TableName.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.JobNo.Contains(input.Filter))
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
						.WhereIf(input.MinDocTypeFilter != null, e => e.DocType >= input.MinDocTypeFilter)
						.WhereIf(input.MaxDocTypeFilter != null, e => e.DocType <= input.MaxDocTypeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DocDescFilter),  e => e.DocDesc == input.DocDescFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter),  e => e.ItemID == input.ItemIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.srnoFilter),  e => e.srno == input.srnoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNITFilter),  e => e.UNIT == input.UNITFilter)
						.WhereIf(input.MinConverFilter != null, e => e.Conver >= input.MinConverFilter)
						.WhereIf(input.MaxConverFilter != null, e => e.Conver <= input.MaxConverFilter)
						.WhereIf(input.MinQtyFilter != null, e => e.Qty >= input.MinQtyFilter)
						.WhereIf(input.MaxQtyFilter != null, e => e.Qty <= input.MaxQtyFilter)
						.WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
						.WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter),  e => e.Descp == input.DescpFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TableNameFilter),  e => e.TableName == input.TableNameFilter)
						.WhereIf(input.MinPKIDFilter != null, e => e.PKID >= input.MinPKIDFilter)
						.WhereIf(input.MaxPKIDFilter != null, e => e.PKID <= input.MaxPKIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobNoFilter),  e => e.JobNo == input.JobNoFilter);

			var pagedAndFilteredICLEDG = filteredICLEDG
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var icledg = from o in pagedAndFilteredICLEDG
                         select new GetICLEDGForViewDto() {
							ICLEDG = new ICLEDGDto
							{
                                DocDate = o.DocDate,
                                DocType = o.DocType,
                                DocDesc = o.DocDesc,
                                DocNo = o.DocNo,
                                LocID = o.LocID,
                                ItemID = o.ItemID,
                                srno = o.srno,
                                UNIT = o.UNIT,
                                Conver = o.Conver,
                                Qty = o.Qty,
                                Rate = o.Rate,
                                Amount = o.Amount,
                                Descp = o.Descp,
                                TableName = o.TableName,
                                PKID = o.PKID,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                JobNo = o.JobNo,
                                Id = o.Id
							}
						};

            var totalCount = await filteredICLEDG.CountAsync();

            return new PagedResultDto<GetICLEDGForViewDto>(
                totalCount,
                await icledg.ToListAsync()
            );
         }
		 
		 public async Task<GetICLEDGForViewDto> GetICLEDGForView(int id)
         {
            var icledg = await _icledgRepository.GetAsync(id);

            var output = new GetICLEDGForViewDto { ICLEDG = ObjectMapper.Map<ICLEDGDto>(icledg) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_ICLEDG_Edit)]
		 public async Task<GetICLEDGForEditOutput> GetICLEDGForEdit(EntityDto input)
         {
            var icledg = await _icledgRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetICLEDGForEditOutput {ICLEDG = ObjectMapper.Map<CreateOrEditICLEDGDto>(icledg)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditICLEDGDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICLEDG_Create)]
		 protected virtual async Task Create(CreateOrEditICLEDGDto input)
         {
            var icledg = ObjectMapper.Map<ICLEDG>(input);

			
			if (AbpSession.TenantId != null)
			{
				icledg.TenantId = (int) AbpSession.TenantId;
			}
		

            await _icledgRepository.InsertAsync(icledg);
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICLEDG_Edit)]
		 protected virtual async Task Update(CreateOrEditICLEDGDto input)
         {
            var icledg = await _icledgRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, icledg);
         }

		 [AbpAuthorize(AppPermissions.Inventory_ICLEDG_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _icledgRepository.DeleteAsync(input.Id);
         } 
    }
}
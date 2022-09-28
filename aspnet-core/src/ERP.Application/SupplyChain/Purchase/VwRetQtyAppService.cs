

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Purchase.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Purchase
{
	[AbpAuthorize(AppPermissions.Pages_VwRetQty)]
    public class VwRetQtyAppService : ERPAppServiceBase, IVwRetQtyAppService
    {
		 private readonly IRepository<VwRetQty> _vwRetQtyRepository;
		 

		  public VwRetQtyAppService(IRepository<VwRetQty> vwRetQtyRepository ) 
		  {
			_vwRetQtyRepository = vwRetQtyRepository;
			
		  }

		 public async Task<PagedResultDto<GetVwRetQtyForViewDto>> GetAll(GetAllVwRetQtyInput input)
         {
			
			var filteredVwRetQty = _vwRetQtyRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ItemID.Contains(input.Filter) || e.Unit.Contains(input.Filter) || e.Remarks.Contains(input.Filter) || e.descp.Contains(input.Filter))
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter),  e => e.ItemID == input.ItemIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter),  e => e.Unit == input.UnitFilter)
						.WhereIf(input.MinConverFilter != null, e => e.Conver >= input.MinConverFilter)
						.WhereIf(input.MaxConverFilter != null, e => e.Conver <= input.MaxConverFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter),  e => e.Remarks == input.RemarksFilter)
						.WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
						.WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter)
						.WhereIf(input.MinQtyFilter != null, e => e.Qty >= input.MinQtyFilter)
						.WhereIf(input.MaxQtyFilter != null, e => e.Qty <= input.MaxQtyFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(input.MinqtypFilter != null, e => e.qtyp >= input.MinqtypFilter)
						.WhereIf(input.MaxqtypFilter != null, e => e.qtyp <= input.MaxqtypFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.descpFilter),  e => e.descp == input.descpFilter);

			var pagedAndFilteredVwRetQty = filteredVwRetQty
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var vwRetQty = from o in pagedAndFilteredVwRetQty
                         select new GetVwRetQtyForViewDto() {
							VwRetQty = new VwRetQtyDto
							{
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                ItemID = o.ItemID,
                                Unit = o.Unit,
                                Conver = o.Conver,
                                Remarks = o.Remarks,
                                Rate = o.Rate,
                                Qty = o.Qty,
                                Amount = o.Amount,
                                qtyp = o.qtyp,
                                descp = o.descp,
                                Id = o.Id
							}
						};

            var totalCount = await filteredVwRetQty.CountAsync();

            return new PagedResultDto<GetVwRetQtyForViewDto>(
                totalCount,
                await vwRetQty.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_VwRetQty_Edit)]
		 public async Task<GetVwRetQtyForEditOutput> GetVwRetQtyForEdit(EntityDto input)
         {
            var vwRetQty = await _vwRetQtyRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetVwRetQtyForEditOutput {VwRetQty = ObjectMapper.Map<CreateOrEditVwRetQtyDto>(vwRetQty)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditVwRetQtyDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_VwRetQty_Create)]
		 protected virtual async Task Create(CreateOrEditVwRetQtyDto input)
         {
            var vwRetQty = ObjectMapper.Map<VwRetQty>(input);

			
			if (AbpSession.TenantId != null)
			{
				vwRetQty.TenantId = (int) AbpSession.TenantId;
			}
		

            await _vwRetQtyRepository.InsertAsync(vwRetQty);
         }

		 [AbpAuthorize(AppPermissions.Pages_VwRetQty_Edit)]
		 protected virtual async Task Update(CreateOrEditVwRetQtyDto input)
         {
            var vwRetQty = await _vwRetQtyRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, vwRetQty);
         }

		 [AbpAuthorize(AppPermissions.Pages_VwRetQty_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _vwRetQtyRepository.DeleteAsync(input.Id);
         } 
    }
}
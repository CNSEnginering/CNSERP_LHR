

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
	[AbpAuthorize(AppPermissions.Pages_POSTAT)]
    public class POSTATAppService : ERPAppServiceBase, IPOSTATAppService
    {
		 private readonly IRepository<POSTAT> _postatRepository;
		 

		  public POSTATAppService(IRepository<POSTAT> postatRepository ) 
		  {
			_postatRepository = postatRepository;
			
		  }

		 public async Task<PagedResultDto<GetPOSTATForViewDto>> GetAll(GetAllPOSTATInput input)
         {
			
			var filteredPOSTAT = _postatRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ItemID.Contains(input.Filter) || e.Unit.Contains(input.Filter) || e.Remarks.Contains(input.Filter))
						.WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
						.WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter),  e => e.ItemID == input.ItemIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter),  e => e.Unit == input.UnitFilter)
						.WhereIf(input.MinConverFilter != null, e => e.Conver >= input.MinConverFilter)
						.WhereIf(input.MaxConverFilter != null, e => e.Conver <= input.MaxConverFilter)
						.WhereIf(input.MinQtyFilter != null, e => e.Qty >= input.MinQtyFilter)
						.WhereIf(input.MaxQtyFilter != null, e => e.Qty <= input.MaxQtyFilter)
						.WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
						.WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter),  e => e.Remarks == input.RemarksFilter)
						.WhereIf(input.MinReceivedFilter != null, e => e.Received >= input.MinReceivedFilter)
						.WhereIf(input.MaxReceivedFilter != null, e => e.Received <= input.MaxReceivedFilter)
						.WhereIf(input.MinReturnedFilter != null, e => e.Returned >= input.MinReturnedFilter)
						.WhereIf(input.MaxReturnedFilter != null, e => e.Returned <= input.MaxReturnedFilter)
						.WhereIf(input.MinReqNoFilter != null, e => e.ReqNo >= input.MinReqNoFilter)
						.WhereIf(input.MaxReqNoFilter != null, e => e.ReqNo <= input.MaxReqNoFilter)
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter);

			var pagedAndFilteredPOSTAT = filteredPOSTAT
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var postat = from o in pagedAndFilteredPOSTAT
                         select new GetPOSTATForViewDto() {
							POSTAT = new POSTATDto
							{
                                DetID = o.DetID,
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                ItemID = o.ItemID,
                                Unit = o.Unit,
                                Conver = o.Conver,
                                Qty = o.Qty,
                                Rate = o.Rate,
                                Amount = o.Amount,
                                Remarks = o.Remarks,
                                Received = o.Received,
                                Returned = o.Returned,
                                ReqNo = o.ReqNo,
                                DocDate = o.DocDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPOSTAT.CountAsync();

            return new PagedResultDto<GetPOSTATForViewDto>(
                totalCount,
                await postat.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_POSTAT_Edit)]
		 public async Task<GetPOSTATForEditOutput> GetPOSTATForEdit(EntityDto input)
         {
            var postat = await _postatRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPOSTATForEditOutput {POSTAT = ObjectMapper.Map<CreateOrEditPOSTATDto>(postat)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPOSTATDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_POSTAT_Create)]
		 protected virtual async Task Create(CreateOrEditPOSTATDto input)
         {
            var postat = ObjectMapper.Map<POSTAT>(input);

			
			if (AbpSession.TenantId != null)
			{
				postat.TenantId = (int) AbpSession.TenantId;
			}
		

            await _postatRepository.InsertAsync(postat);
         }

		 [AbpAuthorize(AppPermissions.Pages_POSTAT_Edit)]
		 protected virtual async Task Update(CreateOrEditPOSTATDto input)
         {
            var postat = await _postatRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, postat);
         }

		 [AbpAuthorize(AppPermissions.Pages_POSTAT_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _postatRepository.DeleteAsync(input.Id);
         } 
    }
}
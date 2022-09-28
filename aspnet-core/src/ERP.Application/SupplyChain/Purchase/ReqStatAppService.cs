

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

namespace ERP.SupplyChain.Purchase
{
	[AbpAuthorize(AppPermissions.Pages_ReqStat)]
    public class ReqStatAppService : ERPAppServiceBase, IReqStatAppService
    {
		 private readonly IRepository<ReqStat> _reqStatRepository;
		 

		  public ReqStatAppService(IRepository<ReqStat> reqStatRepository ) 
		  {
			_reqStatRepository = reqStatRepository;
			
		  }

		 public async Task<PagedResultDto<GetReqStatForViewDto>> GetAll(GetAllReqStatInput input)
         {
			
			var filteredReqStat = _reqStatRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ItemID.Contains(input.Filter) || e.Unit.Contains(input.Filter) || e.Remarks.Contains(input.Filter))
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
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter),  e => e.Remarks == input.RemarksFilter)
						.WhereIf(input.MinQIHFilter != null, e => e.QIH >= input.MinQIHFilter)
						.WhereIf(input.MaxQIHFilter != null, e => e.QIH <= input.MaxQIHFilter)
						.WhereIf(input.MinPOQtyFilter != null, e => e.POQty >= input.MinPOQtyFilter)
						.WhereIf(input.MaxPOQtyFilter != null, e => e.POQty <= input.MaxPOQtyFilter);

			var pagedAndFilteredReqStat = filteredReqStat
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var reqStat = from o in pagedAndFilteredReqStat
                         select new GetReqStatForViewDto() {
							ReqStat = new ReqStatDto
							{
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                ItemID = o.ItemID,
                                Unit = o.Unit,
                                Conver = o.Conver,
                                Qty = o.Qty,
                                Remarks = o.Remarks,
                                QIH = o.QIH,
                                POQty = o.POQty,
                                Id = o.Id
							}
						};

            var totalCount = await filteredReqStat.CountAsync();

            return new PagedResultDto<GetReqStatForViewDto>(
                totalCount,
                await reqStat.ToListAsync()
            );
         }
		 
		 public async Task<GetReqStatForViewDto> GetReqStatForView(int id)
         {
            var reqStat = await _reqStatRepository.GetAsync(id);

            var output = new GetReqStatForViewDto { ReqStat = ObjectMapper.Map<ReqStatDto>(reqStat) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ReqStat_Edit)]
		 public async Task<GetReqStatForEditOutput> GetReqStatForEdit(EntityDto input)
         {
            var reqStat = await _reqStatRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetReqStatForEditOutput {ReqStat = ObjectMapper.Map<CreateOrEditReqStatDto>(reqStat)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditReqStatDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ReqStat_Create)]
		 protected virtual async Task Create(CreateOrEditReqStatDto input)
         {
            var reqStat = ObjectMapper.Map<ReqStat>(input);

			
			if (AbpSession.TenantId != null)
			{
				reqStat.TenantId = (int) AbpSession.TenantId;
			}
		

            await _reqStatRepository.InsertAsync(reqStat);
         }

		 [AbpAuthorize(AppPermissions.Pages_ReqStat_Edit)]
		 protected virtual async Task Update(CreateOrEditReqStatDto input)
         {
            var reqStat = await _reqStatRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, reqStat);
         }

		 [AbpAuthorize(AppPermissions.Pages_ReqStat_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _reqStatRepository.DeleteAsync(input.Id);
         } 
    }
}
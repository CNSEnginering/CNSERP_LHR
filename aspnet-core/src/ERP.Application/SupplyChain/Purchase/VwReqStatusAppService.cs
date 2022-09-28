

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
	[AbpAuthorize(AppPermissions.Pages_VwReqStatus)]
    public class VwReqStatusAppService : ERPAppServiceBase, IVwReqStatusAppService
    {
		 private readonly IRepository<VwReqStatus> _vwReqStatusRepository;
		 

		  public VwReqStatusAppService(IRepository<VwReqStatus> vwReqStatusRepository ) 
		  {
			_vwReqStatusRepository = vwReqStatusRepository;
			
		  }

		 public async Task<PagedResultDto<GetVwReqStatusForViewDto>> GetAll(GetAllVwReqStatusInput input)
         {
			
			var filteredVwReqStatus = _vwReqStatusRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Narration.Contains(input.Filter) || e.ItemID.Contains(input.Filter) || e.Descp.Contains(input.Filter) || e.party_name.Contains(input.Filter) || e.location.Contains(input.Filter) || e.rec_narration.Contains(input.Filter) || e.OrdNo.Contains(input.Filter))
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter),  e => e.ItemID == input.ItemIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter),  e => e.Descp == input.DescpFilter)
						.WhereIf(input.MinreqqtyFilter != null, e => e.reqqty >= input.MinreqqtyFilter)
						.WhereIf(input.MaxreqqtyFilter != null, e => e.reqqty <= input.MaxreqqtyFilter)
						.WhereIf(input.MinpoqtyFilter != null, e => e.poqty >= input.MinpoqtyFilter)
						.WhereIf(input.MaxpoqtyFilter != null, e => e.poqty <= input.MaxpoqtyFilter)
						.WhereIf(input.MinrecqtyFilter != null, e => e.recqty >= input.MinrecqtyFilter)
						.WhereIf(input.MaxrecqtyFilter != null, e => e.recqty <= input.MaxrecqtyFilter)
						.WhereIf(input.MinpodateFilter != null, e => e.podate >= input.MinpodateFilter)
						.WhereIf(input.MaxpodateFilter != null, e => e.podate <= input.MaxpodateFilter)
						.WhereIf(input.MinrecdateFilter != null, e => e.recdate >= input.MinrecdateFilter)
						.WhereIf(input.MaxrecdateFilter != null, e => e.recdate <= input.MaxrecdateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.party_nameFilter),  e => e.party_name == input.party_nameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.locationFilter),  e => e.location == input.locationFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.rec_narrationFilter),  e => e.rec_narration == input.rec_narrationFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrdNoFilter),  e => e.OrdNo == input.OrdNoFilter)
						.WhereIf(input.MinrecdocnoFilter != null, e => e.recdocno >= input.MinrecdocnoFilter)
						.WhereIf(input.MaxrecdocnoFilter != null, e => e.recdocno <= input.MaxrecdocnoFilter)
						.WhereIf(input.MinpodocnoFilter != null, e => e.podocno >= input.MinpodocnoFilter)
						.WhereIf(input.MaxpodocnoFilter != null, e => e.podocno <= input.MaxpodocnoFilter)
						.WhereIf(input.MinSUBCCIDFilter != null, e => e.SUBCCID >= input.MinSUBCCIDFilter)
						.WhereIf(input.MaxSUBCCIDFilter != null, e => e.SUBCCID <= input.MaxSUBCCIDFilter);

			var pagedAndFilteredVwReqStatus = filteredVwReqStatus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var vwReqStatus = from o in pagedAndFilteredVwReqStatus
                         select new GetVwReqStatusForViewDto() {
							VwReqStatus = new VwReqStatusDto
							{
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                Narration = o.Narration,
                                ItemID = o.ItemID,
                                Descp = o.Descp,
                                reqqty = o.reqqty,
                                poqty = o.poqty,
                                recqty = o.recqty,
                                podate = o.podate,
                                recdate = o.recdate,
                                party_name = o.party_name,
                                location = o.location,
                                rec_narration = o.rec_narration,
                                OrdNo = o.OrdNo,
                                recdocno = o.recdocno,
                                podocno = o.podocno,
                                SUBCCID = o.SUBCCID,
                                Id = o.Id
							}
						};

            var totalCount = await filteredVwReqStatus.CountAsync();

            return new PagedResultDto<GetVwReqStatusForViewDto>(
                totalCount,
                await vwReqStatus.ToListAsync()
            );
         }
		 
		 public async Task<GetVwReqStatusForViewDto> GetVwReqStatusForView(int id)
         {
            var vwReqStatus = await _vwReqStatusRepository.GetAsync(id);

            var output = new GetVwReqStatusForViewDto { VwReqStatus = ObjectMapper.Map<VwReqStatusDto>(vwReqStatus) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_VwReqStatus_Edit)]
		 public async Task<GetVwReqStatusForEditOutput> GetVwReqStatusForEdit(EntityDto input)
         {
            var vwReqStatus = await _vwReqStatusRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetVwReqStatusForEditOutput {VwReqStatus = ObjectMapper.Map<CreateOrEditVwReqStatusDto>(vwReqStatus)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditVwReqStatusDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_VwReqStatus_Create)]
		 protected virtual async Task Create(CreateOrEditVwReqStatusDto input)
         {
            var vwReqStatus = ObjectMapper.Map<VwReqStatus>(input);

			
			if (AbpSession.TenantId != null)
			{
				vwReqStatus.TenantId = (int) AbpSession.TenantId;
			}
		

            await _vwReqStatusRepository.InsertAsync(vwReqStatus);
         }

		 [AbpAuthorize(AppPermissions.Pages_VwReqStatus_Edit)]
		 protected virtual async Task Update(CreateOrEditVwReqStatusDto input)
         {
            var vwReqStatus = await _vwReqStatusRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, vwReqStatus);
         }

		 [AbpAuthorize(AppPermissions.Pages_VwReqStatus_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _vwReqStatusRepository.DeleteAsync(input.Id);
         } 
    }
}
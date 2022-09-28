

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
	[AbpAuthorize(AppPermissions.Pages_VwReqStatus2)]
    public class VwReqStatus2AppService : ERPAppServiceBase, IVwReqStatus2AppService
    {
		 private readonly IRepository<VwReqStatus2> _vwReqStatus2Repository;
		 

		  public VwReqStatus2AppService(IRepository<VwReqStatus2> vwReqStatus2Repository ) 
		  {
			_vwReqStatus2Repository = vwReqStatus2Repository;
			
		  }

		 public async Task<PagedResultDto<GetVwReqStatus2ForViewDto>> GetAll(GetAllVwReqStatus2Input input)
         {
			
			var filteredVwReqStatus2 = _vwReqStatus2Repository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ItemID.Contains(input.Filter) || e.Descp.Contains(input.Filter) || e.Unit.Contains(input.Filter))
						.WhereIf(input.MinLocidFilter != null, e => e.Locid >= input.MinLocidFilter)
						.WhereIf(input.MaxLocidFilter != null, e => e.Locid <= input.MaxLocidFilter)
						.WhereIf(input.MinDocnoFilter != null, e => e.Docno >= input.MinDocnoFilter)
						.WhereIf(input.MaxDocnoFilter != null, e => e.Docno <= input.MaxDocnoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter),  e => e.ItemID == input.ItemIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter),  e => e.Descp == input.DescpFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter),  e => e.Unit == input.UnitFilter)
						.WhereIf(input.MinConverFilter != null, e => e.Conver >= input.MinConverFilter)
						.WhereIf(input.MaxConverFilter != null, e => e.Conver <= input.MaxConverFilter)
						.WhereIf(input.MinReqQtyFilter != null, e => e.ReqQty >= input.MinReqQtyFilter)
						.WhereIf(input.MaxReqQtyFilter != null, e => e.ReqQty <= input.MaxReqQtyFilter)
						.WhereIf(input.MinQIHFilter != null, e => e.QIH >= input.MinQIHFilter)
						.WhereIf(input.MaxQIHFilter != null, e => e.QIH <= input.MaxQIHFilter)
						.WhereIf(input.MinPOQtyFilter != null, e => e.POQty >= input.MinPOQtyFilter)
						.WhereIf(input.MaxPOQtyFilter != null, e => e.POQty <= input.MaxPOQtyFilter)
						.WhereIf(input.MinReceivedFilter != null, e => e.Received >= input.MinReceivedFilter)
						.WhereIf(input.MaxReceivedFilter != null, e => e.Received <= input.MaxReceivedFilter)
						.WhereIf(input.MinReturnedFilter != null, e => e.Returned >= input.MinReturnedFilter)
						.WhereIf(input.MaxReturnedFilter != null, e => e.Returned <= input.MaxReturnedFilter)
						.WhereIf(input.MinQtyPFilter != null, e => e.QtyP >= input.MinQtyPFilter)
						.WhereIf(input.MaxQtyPFilter != null, e => e.QtyP <= input.MaxQtyPFilter);

			var pagedAndFilteredVwReqStatus2 = filteredVwReqStatus2
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var vwReqStatus2 = from o in pagedAndFilteredVwReqStatus2
                         select new GetVwReqStatus2ForViewDto() {
							VwReqStatus2 = new VwReqStatus2Dto
							{
                                Locid = o.Locid,
                                Docno = o.Docno,
                                ItemID = o.ItemID,
                                Descp = o.Descp,
                                Unit = o.Unit,
                                Conver = o.Conver,
                                ReqQty = o.ReqQty,
                                QIH = o.QIH,
                                POQty = o.POQty,
                                Received = o.Received,
                                Returned = o.Returned,
                                QtyP = o.QtyP,
                                Id = o.Id
							}
						};

            var totalCount = await filteredVwReqStatus2.CountAsync();

            return new PagedResultDto<GetVwReqStatus2ForViewDto>(
                totalCount,
                await vwReqStatus2.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_VwReqStatus2_Edit)]
		 public async Task<GetVwReqStatus2ForEditOutput> GetVwReqStatus2ForEdit(EntityDto input)
         {
            var vwReqStatus2 = await _vwReqStatus2Repository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetVwReqStatus2ForEditOutput {VwReqStatus2 = ObjectMapper.Map<CreateOrEditVwReqStatus2Dto>(vwReqStatus2)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditVwReqStatus2Dto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_VwReqStatus2_Create)]
		 protected virtual async Task Create(CreateOrEditVwReqStatus2Dto input)
         {
            var vwReqStatus2 = ObjectMapper.Map<VwReqStatus2>(input);

			
			if (AbpSession.TenantId != null)
			{
				vwReqStatus2.TenantId = (int) AbpSession.TenantId;
			}
		

            await _vwReqStatus2Repository.InsertAsync(vwReqStatus2);
         }

		 [AbpAuthorize(AppPermissions.Pages_VwReqStatus2_Edit)]
		 protected virtual async Task Update(CreateOrEditVwReqStatus2Dto input)
         {
            var vwReqStatus2 = await _vwReqStatus2Repository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, vwReqStatus2);
         }

		 [AbpAuthorize(AppPermissions.Pages_VwReqStatus2_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _vwReqStatus2Repository.DeleteAsync(input.Id);
         } 
    }
}
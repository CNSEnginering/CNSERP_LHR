

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.AccountReceivables.RouteInvoices.Exporting;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.AccountReceivables.RouteInvoices
{
	[AbpAuthorize(AppPermissions.Pages_ARINVD)]
    public class ARINVDAppService : ERPAppServiceBase, IARINVDAppService
    {
		 private readonly IRepository<ARINVD> _arinvdRepository;
		 private readonly IARINVDExcelExporter _arinvdExcelExporter;
		 

		  public ARINVDAppService(IRepository<ARINVD> arinvdRepository, IARINVDExcelExporter arinvdExcelExporter ) 
		  {
			_arinvdRepository = arinvdRepository;
			_arinvdExcelExporter = arinvdExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetARINVDForViewDto>> GetAll(GetAllARINVDInput input)
         {
			
			var filteredARINVD = _arinvdRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AccountID.Contains(input.Filter) || e.InvNumber.Contains(input.Filter) || e.ChequeNo.Contains(input.Filter) || e.Narration.Contains(input.Filter))
						.WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
						.WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter),  e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
						.WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
						.WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.InvNumberFilter),  e => e.InvNumber.ToLower() == input.InvNumberFilter.ToLower().Trim())
						.WhereIf(input.MinInvAmountFilter != null, e => e.InvAmount >= input.MinInvAmountFilter)
						.WhereIf(input.MaxInvAmountFilter != null, e => e.InvAmount <= input.MaxInvAmountFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.TaxAmountFilter),  e => e.TaxAmount.ToLower() == input.TaxAmountFilter.ToLower().Trim())
						.WhereIf(input.MinRecpAmountFilter != null, e => e.RecpAmount >= input.MinRecpAmountFilter)
						.WhereIf(input.MaxRecpAmountFilter != null, e => e.RecpAmount <= input.MaxRecpAmountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ChequeNoFilter),  e => e.ChequeNo.ToLower() == input.ChequeNoFilter.ToLower().Trim())
						.WhereIf(input.AdjustFilter > -1,  e => Convert.ToInt32(e.Adjust) == input.AdjustFilter )
						.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration.ToLower() == input.NarrationFilter.ToLower().Trim());

			var pagedAndFilteredARINVD = filteredARINVD
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var arinvd = from o in pagedAndFilteredARINVD
                         select new GetARINVDForViewDto() {
							ARINVD = new ARINVDDto
							{
                                DetID = o.DetID,
                                AccountID = o.AccountID,
                                SubAccID = o.SubAccID,
                                DocNo = o.DocNo,
                                InvNumber = o.InvNumber,
                                InvAmount = o.InvAmount,
                                TaxAmount = o.TaxAmount,
                                RecpAmount = o.RecpAmount,
                                ChequeNo = o.ChequeNo,
                                Adjust = o.Adjust,
                                Narration = o.Narration,
                                Id = o.Id
							}
						};

            var totalCount = await filteredARINVD.CountAsync();

            return new PagedResultDto<GetARINVDForViewDto>(
                totalCount,
                await arinvd.ToListAsync()
            );
         }
		 
		 public async Task<GetARINVDForViewDto> GetARINVDForView(int id)
         {
            var arinvd = await _arinvdRepository.GetAsync(id);

            var output = new GetARINVDForViewDto { ARINVD = ObjectMapper.Map<ARINVDDto>(arinvd) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ARINVD_Edit)]
		 public async Task<GetARINVDForEditOutput> GetARINVDForEdit(EntityDto input)
         {
            var arinvd = await _arinvdRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetARINVDForEditOutput {ARINVD = ObjectMapper.Map<CreateOrEditARINVDDto>(arinvd)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditARINVDDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ARINVD_Create)]
         protected virtual async Task Create(CreateOrEditARINVDDto input)
         {
            var arinvd = ObjectMapper.Map<ARINVD>(input);

			
			if (AbpSession.TenantId != null)
			{
				arinvd.TenantId = (int) AbpSession.TenantId;
			}
		

            await _arinvdRepository.InsertAsync(arinvd);
         }

		 [AbpAuthorize(AppPermissions.Pages_ARINVD_Edit)]
		 private async Task Update(CreateOrEditARINVDDto input)
         {
            var arinvd = await _arinvdRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, arinvd);
         }

		 [AbpAuthorize(AppPermissions.Pages_ARINVD_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _arinvdRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetARINVDToExcel(GetAllARINVDForExcelInput input)
         {
			
			var filteredARINVD = _arinvdRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AccountID.Contains(input.Filter) || e.InvNumber.Contains(input.Filter) || e.ChequeNo.Contains(input.Filter) || e.Narration.Contains(input.Filter))
						.WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
						.WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter),  e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
						.WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
						.WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.InvNumberFilter),  e => e.InvNumber.ToLower() == input.InvNumberFilter.ToLower().Trim())
						.WhereIf(input.MinInvAmountFilter != null, e => e.InvAmount >= input.MinInvAmountFilter)
						.WhereIf(input.MaxInvAmountFilter != null, e => e.InvAmount <= input.MaxInvAmountFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.TaxAmountFilter),  e => e.TaxAmount.ToLower() == input.TaxAmountFilter.ToLower().Trim())
						.WhereIf(input.MinRecpAmountFilter != null, e => e.RecpAmount >= input.MinRecpAmountFilter)
						.WhereIf(input.MaxRecpAmountFilter != null, e => e.RecpAmount <= input.MaxRecpAmountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ChequeNoFilter),  e => e.ChequeNo.ToLower() == input.ChequeNoFilter.ToLower().Trim())
						.WhereIf(input.AdjustFilter > -1,  e => Convert.ToInt32(e.Adjust) == input.AdjustFilter )
						.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration.ToLower() == input.NarrationFilter.ToLower().Trim());

			var query = (from o in filteredARINVD
                         select new GetARINVDForViewDto() { 
							ARINVD = new ARINVDDto
							{
                                DetID = o.DetID,
                                AccountID = o.AccountID,
                                SubAccID = o.SubAccID,
                                DocNo = o.DocNo,
                                InvNumber = o.InvNumber,
                                InvAmount = o.InvAmount,
                                TaxAmount = o.TaxAmount,
                                RecpAmount = o.RecpAmount,
                                ChequeNo = o.ChequeNo,
                                Adjust = o.Adjust,
                                Narration = o.Narration,
                                Id = o.Id
							}
						 });


            var arinvdListDtos = await query.ToListAsync();

            return _arinvdExcelExporter.ExportToFile(arinvdListDtos);
         }


    }
}
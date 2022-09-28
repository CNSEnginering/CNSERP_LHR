

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.AccountPayables.Exporting;
using ERP.AccountPayables.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.AccountPayables
{
	[AbpAuthorize(AppPermissions.Pages_APTerms)]
    public class APTermsAppService : ERPAppServiceBase, IAPTermsAppService
    {
		 private readonly IRepository<APTerm> _apTermRepository;
		 private readonly IAPTermsExcelExporter _apTermsExcelExporter;
		 

		  public APTermsAppService(IRepository<APTerm> apTermRepository, IAPTermsExcelExporter apTermsExcelExporter ) 
		  {
			_apTermRepository = apTermRepository;
			_apTermsExcelExporter = apTermsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetAPTermForViewDto>> GetAll(GetAllAPTermsInput input)
         {
			
			var filteredAPTerms = _apTermRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.TERMDESC.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TERMDESCFilter),  e => e.TERMDESC.ToLower() == input.TERMDESCFilter.ToLower().Trim())
						.WhereIf(input.MinTERMRATEFilter != null, e => e.TERMRATE >= input.MinTERMRATEFilter)
						.WhereIf(input.MaxTERMRATEFilter != null, e => e.TERMRATE <= input.MaxTERMRATEFilter)
						.WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
						.WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter),  e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
						.WhereIf(input.INACTIVEFilter > -1,  e => Convert.ToInt32(e.INACTIVE) == input.INACTIVEFilter );

			var pagedAndFilteredAPTerms = filteredAPTerms
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var apTerms = from o in pagedAndFilteredAPTerms
                         select new GetAPTermForViewDto() {
							APTerm = new APTermDto
							{
                                TERMDESC = o.TERMDESC,
                                TERMRATE = o.TERMRATE,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                INACTIVE = o.INACTIVE,
                                TermType=o.TermType,
                                TaxStatus=o.TaxStatus,
                                Id = o.Id
							}
						};

            var totalCount = await filteredAPTerms.CountAsync();

            return new PagedResultDto<GetAPTermForViewDto>(
                totalCount,
                await apTerms.ToListAsync()
            );
         }
		 
		 public async Task<GetAPTermForViewDto> GetAPTermForView(int id)
         {
            var apTerm = await _apTermRepository.GetAsync(id);

            var output = new GetAPTermForViewDto { APTerm = ObjectMapper.Map<APTermDto>(apTerm) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_APTerms_Edit)]
		 public async Task<GetAPTermForEditOutput> GetAPTermForEdit(EntityDto input)
         {
            var apTerm = await _apTermRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAPTermForEditOutput {APTerm = ObjectMapper.Map<CreateOrEditAPTermDto>(apTerm)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAPTermDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_APTerms_Create)]
		 protected virtual async Task Create(CreateOrEditAPTermDto input)
         {
            var apTerm = ObjectMapper.Map<APTerm>(input);

			
			if (AbpSession.TenantId != null)
			{
				apTerm.TenantId = (int) AbpSession.TenantId;
			}
		

            await _apTermRepository.InsertAsync(apTerm);
         }

		 [AbpAuthorize(AppPermissions.Pages_APTerms_Edit)]
		 protected virtual async Task Update(CreateOrEditAPTermDto input)
         {
            var apTerm = await _apTermRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, apTerm);
         }

		 [AbpAuthorize(AppPermissions.Pages_APTerms_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _apTermRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAPTermsToExcel(GetAllAPTermsForExcelInput input)
         {
			
			var filteredAPTerms = _apTermRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.TERMDESC.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TERMDESCFilter),  e => e.TERMDESC.ToLower() == input.TERMDESCFilter.ToLower().Trim())
						.WhereIf(input.MinTERMRATEFilter != null, e => e.TERMRATE >= input.MinTERMRATEFilter)
						.WhereIf(input.MaxTERMRATEFilter != null, e => e.TERMRATE <= input.MaxTERMRATEFilter)
						.WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
						.WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter),  e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
						.WhereIf(input.INACTIVEFilter > -1,  e => Convert.ToInt32(e.INACTIVE) == input.INACTIVEFilter );

			var query = (from o in filteredAPTerms
                         select new GetAPTermForViewDto() { 
							APTerm = new APTermDto
							{
                                TERMDESC = o.TERMDESC,
                                TERMRATE = o.TERMRATE,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                INACTIVE = o.INACTIVE,
                                TermType=o.TermType,
                                TaxStatus=o.TaxStatus,
                                Id = o.Id
							}
						 });


            var apTermListDtos = await query.ToListAsync();
            return _apTermsExcelExporter.ExportToFile(apTermListDtos);
         }


    }
}
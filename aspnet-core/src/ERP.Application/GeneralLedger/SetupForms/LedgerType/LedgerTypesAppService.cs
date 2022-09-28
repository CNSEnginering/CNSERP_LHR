

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.LedgerType.Exporting;
using ERP.GeneralLedger.SetupForms.LedgerType.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.GeneralLedger.SetupForms.LedgerType
{
	[AbpAuthorize(AppPermissions.Pages_LedgerTypes)]
    public class LedgerTypesAppService : ERPAppServiceBase, ILedgerTypesAppService
    {
		 private readonly IRepository<LedgerType> _ledgerTypeRepository;
		 private readonly ILedgerTypesExcelExporter _ledgerTypesExcelExporter;
		 

		  public LedgerTypesAppService(IRepository<LedgerType> ledgerTypeRepository, ILedgerTypesExcelExporter ledgerTypesExcelExporter ) 
		  {
			_ledgerTypeRepository = ledgerTypeRepository;
			_ledgerTypesExcelExporter = ledgerTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetLedgerTypeForViewDto>> GetAll(GetAllLedgerTypesInput input)
         {

            var filteredLedgerTypes = _ledgerTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LedgerDesc.Contains(input.Filter))
                        .WhereIf(input.LedgerIDFilter != null, e => e.LedgerID == input.LedgerIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LedgerDescFilter), e => e.LedgerDesc == input.LedgerDescFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active));

            var pagedAndFilteredLedgerTypes = filteredLedgerTypes
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var ledgerTypes = from o in pagedAndFilteredLedgerTypes
                         select new GetLedgerTypeForViewDto() {
							LedgerType = new LedgerTypeDto
							{
                                LedgerID = o.LedgerID,
                                LedgerDesc = o.LedgerDesc,
                                Active = o.Active,
                                Id = o.Id
							}
						};

            var totalCount = await filteredLedgerTypes.CountAsync();

            return new PagedResultDto<GetLedgerTypeForViewDto>(
                totalCount,
                await ledgerTypes.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LedgerTypes_Edit)]
		 public async Task<GetLedgerTypeForEditOutput> GetLedgerTypeForEdit(EntityDto input)
         {
            var ledgerType = await _ledgerTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLedgerTypeForEditOutput {LedgerType = ObjectMapper.Map<CreateOrEditLedgerTypeDto>(ledgerType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLedgerTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LedgerTypes_Create)]
		 protected virtual async Task Create(CreateOrEditLedgerTypeDto input)
         {
            var ledgerType = ObjectMapper.Map<LedgerType>(input);

			
			if (AbpSession.TenantId != null)
			{
				ledgerType.TenantId = (int) AbpSession.TenantId;
			}

            ledgerType.LedgerID = GetMaxLedgerTypeID();
            await _ledgerTypeRepository.InsertAsync(ledgerType);
         }

		 [AbpAuthorize(AppPermissions.Pages_LedgerTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditLedgerTypeDto input)
         {
            var ledgerType = await _ledgerTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, ledgerType);
         }

		 [AbpAuthorize(AppPermissions.Pages_LedgerTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _ledgerTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLedgerTypesToExcel(GetAllLedgerTypesForExcelInput input)
         {
			
			var filteredLedgerTypes = _ledgerTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false   || e.LedgerDesc.Contains(input.Filter))
                        .WhereIf(input.LedgerIDFilter != null, e => e.LedgerID >= input.LedgerIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LedgerDescFilter),  e => e.LedgerDesc == input.LedgerDescFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) );

			var query = (from o in filteredLedgerTypes
                         select new GetLedgerTypeForViewDto() { 
							LedgerType = new LedgerTypeDto
							{
                                LedgerID = o.LedgerID,
                                LedgerDesc = o.LedgerDesc,
                                Active = o.Active,
                                Id = o.Id
							}
						 });


            var ledgerTypeListDtos = await query.ToListAsync();

            return _ledgerTypesExcelExporter.ExportToFile(ledgerTypeListDtos);
         }

        public async Task<ListResultDto<ComboboxItemDto>> GetLedgerTypesForCombobox(string input)
        {
            var query = _ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var LedgerType = await query.ToListAsync();

            var LedgerTypeDtoList = new List<ComboboxItemDto>();

            foreach (var types in LedgerType)
            {
                LedgerTypeDtoList.Add(new ComboboxItemDto
                {

                    Value = types.LedgerID.ToString(),
                    DisplayText = types.LedgerDesc
                });
            }
            return new ListResultDto<ComboboxItemDto>(
               LedgerTypeDtoList
            );
        }

        public int GetMaxLedgerTypeID()
        {
            var maxid = ((from tab1 in _ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.LedgerID).Max() ?? 0) + 1;
            return maxid;
        }
    }
}
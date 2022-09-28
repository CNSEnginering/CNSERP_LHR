

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Exporting;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Inventory
{
	[AbpAuthorize(AppPermissions.Inventory_TransactionTypes)]
    public class TransactionTypesAppService : ERPAppServiceBase, ITransactionTypesAppService
    {
		 private readonly IRepository<TransactionType> _transactionTypeRepository;
		 private readonly ITransactionTypesExcelExporter _transactionTypesExcelExporter;
		 

		  public TransactionTypesAppService(IRepository<TransactionType> transactionTypeRepository, ITransactionTypesExcelExporter transactionTypesExcelExporter ) 
		  {
			_transactionTypeRepository = transactionTypeRepository;
			_transactionTypesExcelExporter = transactionTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<TransactionTypeDto>> GetAll(GetAllTransactionTypesInput input)
         {
			
			var filteredTransactionTypes = _transactionTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.TypeId.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TypeIdFilter),  e => e.TypeId == input.TypeIdFilter);

			var pagedAndFilteredTransactionTypes = filteredTransactionTypes
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var transactionTypes = from o in pagedAndFilteredTransactionTypes
                         select new TransactionTypeDto() {
                                Description = o.Description,
                                Active = o.Active,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                TypeId = o.TypeId,
                                Id = o.Id
						};

            var totalCount = await filteredTransactionTypes.CountAsync();

            return new PagedResultDto<TransactionTypeDto>(
                totalCount,
                await transactionTypes.ToListAsync()
            );
         }
		 
		 public async Task<TransactionTypeDto> GetTransactionTypeForView(int id)
         {
            var transactionType = await _transactionTypeRepository.GetAsync(id);

            var output = ObjectMapper.Map<TransactionTypeDto>(transactionType) ;
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_TransactionTypes_Edit)]
		 public async Task<TransactionTypeDto> GetTransactionTypeForEdit(EntityDto input)
         {
            var transactionType = await _transactionTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output =  ObjectMapper.Map<TransactionTypeDto>(transactionType);
			
            return output;
         }

		 public async Task CreateOrEdit(TransactionTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Inventory_TransactionTypes_Create)]
		 protected virtual async Task Create(TransactionTypeDto input)
         {
            var transactionType = ObjectMapper.Map<TransactionType>(input);

			
			if (AbpSession.TenantId != null)
			{
				transactionType.TenantId = (int) AbpSession.TenantId;
			}
		

            await _transactionTypeRepository.InsertAsync(transactionType);
         }

		 [AbpAuthorize(AppPermissions.Inventory_TransactionTypes_Edit)]
		 protected virtual async Task Update(TransactionTypeDto input)
         {
            var transactionType = await _transactionTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, transactionType);
         }

		 [AbpAuthorize(AppPermissions.Inventory_TransactionTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _transactionTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTransactionTypesToExcel(GetAllTransactionTypesInput input)
         {
			
			var filteredTransactionTypes = _transactionTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.TypeId.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TypeIdFilter),  e => e.TypeId == input.TypeIdFilter);

			var query = (from o in filteredTransactionTypes
                         select new TransactionTypeDto() { 
                                Description = o.Description,
                                Active = o.Active,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                TypeId = o.TypeId,
                                Id = o.Id
						 });


            var transactionTypeListDtos = await query.ToListAsync();

            return _transactionTypesExcelExporter.ExportToFile(transactionTypeListDtos);
         }


    }
}
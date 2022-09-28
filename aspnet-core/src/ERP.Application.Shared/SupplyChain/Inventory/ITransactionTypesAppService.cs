using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface ITransactionTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<TransactionTypeDto>> GetAll(GetAllTransactionTypesInput input);

        Task<TransactionTypeDto> GetTransactionTypeForView(int id);

		Task<TransactionTypeDto> GetTransactionTypeForEdit(EntityDto input);

		Task CreateOrEdit(TransactionTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTransactionTypesToExcel(GetAllTransactionTypesInput input);

		
    }
}
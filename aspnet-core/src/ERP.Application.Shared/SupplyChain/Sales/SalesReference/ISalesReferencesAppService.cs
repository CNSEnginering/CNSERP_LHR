using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.SalesReference.Dtos;
using ERP.Dto;


namespace ERP.SupplyChain.Sales.SalesReference
{
    public interface ISalesReferencesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSalesReferenceForViewDto>> GetAll(GetAllSalesReferencesInput input);

        Task<GetSalesReferenceForViewDto> GetSalesReferenceForView(int id);

		Task<GetSalesReferenceForEditOutput> GetSalesReferenceForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSalesReferenceDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSalesReferencesToExcel(GetAllSalesReferencesForExcelInput input);

		
    }
}
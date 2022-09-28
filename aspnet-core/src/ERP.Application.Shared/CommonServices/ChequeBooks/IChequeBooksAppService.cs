using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.ChequeBooks.Dtos;
using ERP.Dto;


namespace ERP.CommonServices.ChequeBooks
{
    public interface IChequeBooksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChequeBookForViewDto>> GetAll(GetAllChequeBooksInput input);

        Task<GetChequeBookForViewDto> GetChequeBookForView(int id);

		Task<GetChequeBookForEditOutput> GetChequeBookForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditChequeBookDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetChequeBooksToExcel(GetAllChequeBooksForExcelInput input);

		
    }
}
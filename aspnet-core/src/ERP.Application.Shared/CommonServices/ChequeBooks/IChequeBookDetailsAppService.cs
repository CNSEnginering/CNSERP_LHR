using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.ChequeBooks.Dtos;
using ERP.Dto;


namespace ERP.CommonServices.ChequeBooks
{
    public interface IChequeBookDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetChequeBookDetailForViewDto>> GetAll(GetAllChequeBookDetailsInput input);

        Task<GetChequeBookDetailForViewDto> GetChequeBookDetailForView(int id);

        Task<GetChequeBookDetailForEditOutput> GetChequeBookDetailForEdit(int ID);

        Task CreateOrEdit(ICollection<CreateOrEditChequeBookDetailDto> input);

        Task Delete(int input);

        Task<FileDto> GetChequeBookDetailsToExcel(GetAllChequeBookDetailsForExcelInput input);


    }
}
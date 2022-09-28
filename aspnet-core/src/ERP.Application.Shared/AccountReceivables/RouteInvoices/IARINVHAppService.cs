using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.Dto;

namespace ERP.AccountReceivables.RouteInvoices
{
    public interface IARINVHAppService : IApplicationService 
    {
        Task<PagedResultDto<GetARINVHForViewDto>> GetAll(GetAllARINVHInput input);

        Task<GetARINVHForViewDto> GetARINVHForView(int id);

		Task<CreateOrEditARINVHDto> GetARINVHForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditARINVHDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetARINVHToExcel(GetAllARINVHForExcelInput input);

		
    }
}
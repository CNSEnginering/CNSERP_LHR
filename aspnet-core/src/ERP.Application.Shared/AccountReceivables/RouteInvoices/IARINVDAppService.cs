using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.Dto;

namespace ERP.AccountReceivables.RouteInvoices
{
    public interface IARINVDAppService : IApplicationService 
    {
        Task<PagedResultDto<GetARINVDForViewDto>> GetAll(GetAllARINVDInput input);

        Task<GetARINVDForViewDto> GetARINVDForView(int id);

		Task<GetARINVDForEditOutput> GetARINVDForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditARINVDDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetARINVDToExcel(GetAllARINVDForExcelInput input);

		
    }
}
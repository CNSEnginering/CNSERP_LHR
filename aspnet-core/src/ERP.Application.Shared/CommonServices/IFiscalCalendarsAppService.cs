using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.Dtos;
using ERP.Dto;

namespace ERP.CommonServices
{
    public interface IFiscalCalendarsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFiscalCalendarForViewDto>> GetAll(GetAllFiscalCalendarsInput input);

        Task<GetFiscalCalendarForViewDto> GetFiscalCalendarForView(int id);

		Task<GetFiscalCalendarForEditOutput> GetFiscalCalendarForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditFiscalCalendarDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetFiscalCalendarsToExcel(GetAllFiscalCalendarsForExcelInput input);

		
    }
}
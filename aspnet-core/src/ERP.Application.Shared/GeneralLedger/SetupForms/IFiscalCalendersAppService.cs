using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IFiscalCalendersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFiscalCalenderForViewDto>> GetAll(GetAllFiscalCalendersInput input);

        Task<GetFiscalCalenderForViewDto> GetFiscalCalenderForView(int id);

		Task<GetFiscalCalenderForEditOutput> GetFiscalCalenderForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditFiscalCalenderDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetFiscalCalendersToExcel(GetAllFiscalCalendersForExcelInput input);

        Task<GetFiscalCalenderForEditOutput> GetLastYear();

        Task DeleteCalendar(int input);

        Task UpdateCalender(CreateOrEditFiscalCalenderDto input);

        int CheckCalendarStatus(int input);

        bool CalendarStatus(int? period);

        bool GetFiscalYearStatus(DateTime date, string fiscalYear);

    }
}
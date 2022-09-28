using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Holidays.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.Holidays
{
    public interface IHolidaysAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHolidaysForViewDto>> GetAll(GetAllHolidaysInput input);

        Task<GetHolidaysForViewDto> GetHolidaysForView(int id);

		Task<GetHolidaysForEditOutput> GetHolidaysForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditHolidaysDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetHolidaysToExcel(GetAllHolidaysForExcelInput input);

		
    }
}
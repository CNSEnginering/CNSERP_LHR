using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Education.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Education
{
    public interface IEducationAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEducationForViewDto>> GetAll(GetAllEducationInput input);

		Task<GetEducationForEditOutput> GetEducationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEducationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEducationToExcel(GetAllEducationForExcelInput input);

		
    }
}
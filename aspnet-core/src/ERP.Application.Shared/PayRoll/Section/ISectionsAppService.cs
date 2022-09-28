using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Section.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Section
{
    public interface ISectionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSectionForViewDto>> GetAll(GetAllSectionsInput input);

		Task<GetSectionForEditOutput> GetSectionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSectionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSectionsToExcel(GetAllSectionsForExcelInput input);

		
    }
}
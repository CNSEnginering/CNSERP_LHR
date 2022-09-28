using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IControlDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetControlDetailForViewDto>> GetAll(GetAllControlDetailsInput input);

        Task<GetControlDetailForViewDto> GetControlDetailForView(int id);

		Task<GetControlDetailForEditOutput> GetControlDetailForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditControlDetailDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetControlDetailsToExcel(GetAllControlDetailsForExcelInput input);
		
    }
}
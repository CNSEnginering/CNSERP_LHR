using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountPayables.Setup.ApSetup.Dtos;
using ERP.Dto;

namespace ERP.AccountPayables.Setup.ApSetup
{
    public interface IApSetupsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetApSetupForViewDto>> GetAll(GetAllApSetupsInput input);

        Task<GetApSetupForViewDto> GetApSetupForView(int id);

		Task<GetApSetupForEditOutput> GetApSetupForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditApSetupDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetApSetupsToExcel(GetAllApSetupsForExcelInput input);

		
    }
}
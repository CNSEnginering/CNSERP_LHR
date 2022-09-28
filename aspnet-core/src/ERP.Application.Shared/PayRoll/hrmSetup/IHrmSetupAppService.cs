using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.hrmSetup.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.hrmSetup
{
    public interface IHrmSetupAppService : IApplicationService
    {
        Task<PagedResultDto<GetHrmSetupForViewDto>> GetAll(GetAllHrmSetupInput input);

        Task<GetHrmSetupForEditOutput> GetHrmSetupForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditHrmSetupDto input);

        Task Delete();
        //Task Delete(EntityDto input);

    }
}
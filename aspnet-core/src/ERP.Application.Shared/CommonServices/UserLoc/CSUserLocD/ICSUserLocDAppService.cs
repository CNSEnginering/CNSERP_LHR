using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.UserLoc.CSUserLocD.Dtos;
using ERP.Dto;

namespace ERP.CommonServices.UserLoc.CSUserLocD
{
    public interface ICSUserLocDAppService : IApplicationService
    {
        Task<PagedResultDto<GetCSUserLocDForViewDto>> GetAll(GetAllCSUserLocDInput input);

        Task<GetCSUserLocDForViewDto> GetCSUserLocDForView(int id);

        Task<GetCSUserLocDForEditOutput> GetCSUserLocDForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCSUserLocDDto input);

        Task Delete(EntityDto input);

    }
}
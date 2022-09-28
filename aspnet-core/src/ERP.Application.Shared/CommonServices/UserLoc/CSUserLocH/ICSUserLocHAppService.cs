using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.UserLoc.CSUserLocH.Dtos;
using ERP.Dto;

namespace ERP.CommonServices.UserLoc.CSUserLocH
{
    public interface ICSUserLocHAppService : IApplicationService
    {
        Task<PagedResultDto<GetCSUserLocHForViewDto>> GetAll(GetAllCSUserLocHInput input);

        Task<GetCSUserLocHForViewDto> GetCSUserLocHForView(int id);

        Task<GetCSUserLocHForEditOutput> GetCSUserLocHForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCSUserLocHDto input);

        Task Delete(EntityDto input);

    }
}
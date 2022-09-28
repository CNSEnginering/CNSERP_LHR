using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Cader.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Cader
{
    public interface ICaderAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaderForViewDto>> GetAll(GetAllCaderInput input);

        Task<GetCaderForViewDto> GetCaderForView(int id);

        Task<GetCaderForEditOutput> GetCaderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaderDto input);

        Task Delete(EntityDto input);

    }
}
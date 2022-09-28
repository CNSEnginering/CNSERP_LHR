using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.CaderMaster.cader_link_D.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.CaderMaster.cader_link_D
{
    public interface ICader_link_DAppService : IApplicationService
    {
        Task<PagedResultDto<GetCader_link_DForViewDto>> GetAll(GetAllCader_link_DInput input);

        Task<GetCader_link_DForViewDto> GetCader_link_DForView(int id);

        Task<GetCader_link_DForEditOutput> GetCader_link_DForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCader_link_DDto input);

        Task Delete(EntityDto input);

    }
}
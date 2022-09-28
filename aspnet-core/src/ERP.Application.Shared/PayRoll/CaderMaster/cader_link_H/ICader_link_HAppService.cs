using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.CaderMaster.cader_link_H.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.CaderMaster.cader_link_H
{
    public interface ICader_link_HAppService : IApplicationService
    {
        Task<PagedResultDto<GetCader_link_HForViewDto>> GetAll(GetAllCader_link_HInput input);

        Task<GetCader_link_HForViewDto> GetCader_link_HForView(int id);

        Task<GetCader_link_HForEditOutput> GetCader_link_HForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCader_link_HDto input);

        Task Delete(EntityDto input);

    }
}
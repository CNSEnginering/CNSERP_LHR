using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.CSAlertInfo.Dtos;


namespace ERP.Common.CSAlertInfo
{
    public interface ICSAlertAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCSAlertForViewDto>> GetAll(GetAllCSAlertInput input);

		Task<GetCSAlertForEditOutput> GetCSAlertForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCSAlertDto input);

		Task Delete(EntityDto input);

		
    }
}
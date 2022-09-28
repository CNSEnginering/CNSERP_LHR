using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.AlertLog.Dtos;
using ERP.Dto;


namespace ERP.Common.AlertLog
{
    public interface ICSAlertLogAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCSAlertLogForViewDto>> GetAll(GetAllCSAlertLogInput input);

		Task<GetCSAlertLogForEditOutput> GetCSAlertLogForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCSAlertLogDto input);

		Task Delete(EntityDto input);

		
    }
}
using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.AuditPostingLogs.Dtos;
using ERP.Dto;


namespace ERP.Common.AuditPostingLogs
{
    public interface IAuditPostingLogsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAuditPostingLogsForViewDto>> GetAll(GetAllAuditPostingLogsInput input);

        Task<GetAuditPostingLogsForViewDto> GetAuditPostingLogsForView(int id);

		Task<GetAuditPostingLogsForEditOutput> GetAuditPostingLogsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAuditPostingLogsDto input);

		Task Delete(EntityDto input);

		
    }
}


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Common.AuditPostingLogs.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;

namespace ERP.Common.AuditPostingLogs
{
    [AbpAuthorize(AppPermissions.Pages_AuditPostingLogs)]
    public class AuditPostingLogsAppService : ERPAppServiceBase, IAuditPostingLogsAppService
    {
        private readonly IRepository<AuditPostingLogs> _auditPostingLogsRepository;

        private readonly IRepository<GLTRHeader> _repository;
        public AuditPostingLogsAppService(IRepository<AuditPostingLogs> auditPostingLogsRepository,
            IRepository<GLTRHeader> repository
            )
        {
            _auditPostingLogsRepository = auditPostingLogsRepository;
            _repository = repository;

        }

        public async Task<PagedResultDto<GetAuditPostingLogsForViewDto>> GetAll(GetAllAuditPostingLogsInput input)
        {

            var filteredAuditPostingLogs = _auditPostingLogsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Status.Contains(input.Filter) || e.IpAddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status == input.StatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IpAddressFilter), e => e.IpAddress == input.IpAddressFilter);

            var pagedAndFilteredAuditPostingLogs = filteredAuditPostingLogs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var auditPostingLogs = from o in pagedAndFilteredAuditPostingLogs
                                   select new GetAuditPostingLogsForViewDto()
                                   {
                                       AuditPostingLogs = new AuditPostingLogsDto
                                       {
                                           Status = o.Status,
                                           IpAddress = o.IpAddress,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredAuditPostingLogs.CountAsync();

            return new PagedResultDto<GetAuditPostingLogsForViewDto>(
                totalCount,
                await auditPostingLogs.ToListAsync()
            );
        }

        public async Task<GetAuditPostingLogsForViewDto> GetAuditPostingLogsForView(int id)
        {
            var auditPostingLogs = await _auditPostingLogsRepository.GetAsync(id);

            var output = new GetAuditPostingLogsForViewDto { AuditPostingLogs = ObjectMapper.Map<AuditPostingLogsDto>(auditPostingLogs) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AuditPostingLogs_Edit)]
        public async Task<GetAuditPostingLogsForEditOutput> GetAuditPostingLogsForEdit(EntityDto input)
        {
            var auditPostingLogs = await _auditPostingLogsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAuditPostingLogsForEditOutput { AuditPostingLogs = ObjectMapper.Map<CreateOrEditAuditPostingLogsDto>(auditPostingLogs) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAuditPostingLogsDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        public string[] GetLogsDetails()
        {
            string[] logsDetails = new string[2];
            logsDetails[0] = "Approval Required of today's docs : " + _repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
            && o.Approved == false
            && o.Posted == false).Count();

            logsDetails[1] = "Posting Required of today's docs : " + _repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
            && o.Approved == true
            && o.Posted == false).Count();
            return logsDetails;
        }

        [AbpAuthorize(AppPermissions.Pages_AuditPostingLogs_Create)]
        protected virtual async Task Create(CreateOrEditAuditPostingLogsDto input)
        {
            var auditPostingLogs = ObjectMapper.Map<AuditPostingLogs>(input);


            if (AbpSession.TenantId != null)
            {
                auditPostingLogs.TenantId = (int)AbpSession.TenantId;
            }


            await _auditPostingLogsRepository.InsertAsync(auditPostingLogs);
        }

        [AbpAuthorize(AppPermissions.Pages_AuditPostingLogs_Edit)]
        protected virtual async Task Update(CreateOrEditAuditPostingLogsDto input)
        {
            var auditPostingLogs = await _auditPostingLogsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, auditPostingLogs);
        }

        [AbpAuthorize(AppPermissions.Pages_AuditPostingLogs_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _auditPostingLogsRepository.DeleteAsync(input.Id);
        }
    }
}
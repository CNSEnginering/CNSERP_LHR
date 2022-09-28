

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Common.AlertLog.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.Common.AlertLog
{
	[AbpAuthorize(AppPermissions.Pages_CSAlertLog)]
    public class CSAlertLogAppService : ERPAppServiceBase, ICSAlertLogAppService
    {
		 private readonly IRepository<CSAlertLog> _csAlertLogRepository;
		 

		  public CSAlertLogAppService(IRepository<CSAlertLog> csAlertLogRepository ) 
		  {
			_csAlertLogRepository = csAlertLogRepository;
			
		  }

		 public async Task<PagedResultDto<GetCSAlertLogForViewDto>> GetAll(GetAllCSAlertLogInput input)
         {
			
			var filteredCSAlertLog = _csAlertLogRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.UserHost.Contains(input.Filter) || e.LogInUser.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinSentDateFilter != null, e => e.SentDate >= input.MinSentDateFilter)
						.WhereIf(input.MaxSentDateFilter != null, e => e.SentDate <= input.MaxSentDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserHostFilter),  e => e.UserHost == input.UserHostFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LogInUserFilter),  e => e.LogInUser == input.LogInUserFilter)
						.WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
						.WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var pagedAndFilteredCSAlertLog = filteredCSAlertLog
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var csAlertLog = from o in pagedAndFilteredCSAlertLog
                         select new GetCSAlertLogForViewDto() {
							CSAlertLog = new CSAlertLogDto
							{
                                SentDate = o.SentDate,
                                UserHost = o.UserHost,
                                LogInUser = o.LogInUser,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCSAlertLog.CountAsync();

            return new PagedResultDto<GetCSAlertLogForViewDto>(
                totalCount,
                await csAlertLog.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CSAlertLog_Edit)]
		 public async Task<GetCSAlertLogForEditOutput> GetCSAlertLogForEdit(EntityDto input)
         {
            var csAlertLog = await _csAlertLogRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCSAlertLogForEditOutput {CSAlertLog = ObjectMapper.Map<CreateOrEditCSAlertLogDto>(csAlertLog)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCSAlertLogDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CSAlertLog_Create)]
		 protected virtual async Task Create(CreateOrEditCSAlertLogDto input)
         {
            var csAlertLog = ObjectMapper.Map<CSAlertLog>(input);

			
			if (AbpSession.TenantId != null)
			{
				csAlertLog.TenantId = (int) AbpSession.TenantId;
			}
		

            await _csAlertLogRepository.InsertAsync(csAlertLog);
         }

		 [AbpAuthorize(AppPermissions.Pages_CSAlertLog_Edit)]
		 protected virtual async Task Update(CreateOrEditCSAlertLogDto input)
         {
            var csAlertLog = await _csAlertLogRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, csAlertLog);
         }

		 [AbpAuthorize(AppPermissions.Pages_CSAlertLog_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _csAlertLogRepository.DeleteAsync(input.Id);
         } 
    }
}
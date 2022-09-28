using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Common.CSAlertInfo.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.Common.CSAlertInfo
{
	[AbpAuthorize(AppPermissions.Pages_CSAlert)]
    public class CSAlertAppService : ERPAppServiceBase, ICSAlertAppService
    {
		 private readonly IRepository<CSAlert> _csAlertRepository;
		 
		  public CSAlertAppService(IRepository<CSAlert> csAlertRepository ) 
		  {
			_csAlertRepository = csAlertRepository;
			
		  }

		 public async Task<PagedResultDto<GetCSAlertForViewDto>> GetAll(GetAllCSAlertInput input)
         {
			
			var filteredCSAlert = _csAlertRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AlertDesc.Contains(input.Filter) || e.AlertSubject.Contains(input.Filter) || e.AlertBody.Contains(input.Filter) || e.SendToEmail.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.AlertDescFilter),  e => e.AlertDesc == input.AlertDescFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AlertSubjectFilter),  e => e.AlertSubject == input.AlertSubjectFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AlertBodyFilter),  e => e.AlertBody == input.AlertBodyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SendToEmailFilter),  e => e.SendToEmail == input.SendToEmailFilter)
						.WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
						.WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(input.MinAlertIdFilter != null, e => e.AlertId >= input.MinAlertIdFilter)
						.WhereIf(input.MaxAlertIdFilter != null, e => e.AlertId <= input.MaxAlertIdFilter);

			var pagedAndFilteredCSAlert = filteredCSAlert
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var csAlert = from o in pagedAndFilteredCSAlert
                         select new GetCSAlertForViewDto() {
							CSAlert = new CSAlertDto
							{
                                AlertDesc = o.AlertDesc,
                                AlertSubject = o.AlertSubject,
                                AlertBody = o.AlertBody,
                                SendToEmail = o.SendToEmail,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AlertId = o.AlertId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCSAlert.CountAsync();

            return new PagedResultDto<GetCSAlertForViewDto>(
                totalCount,
                await csAlert.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CSAlert_Edit)]
		 public async Task<GetCSAlertForEditOutput> GetCSAlertForEdit(EntityDto input)
         {
            var csAlert = await _csAlertRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCSAlertForEditOutput {CSAlert = ObjectMapper.Map<CreateOrEditCSAlertDto>(csAlert)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCSAlertDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CSAlert_Create)]
		 protected virtual async Task Create(CreateOrEditCSAlertDto input)
         {
            var csAlert = ObjectMapper.Map<CSAlert>(input);

			
			if (AbpSession.TenantId != null)
			{
				csAlert.TenantId = (int) AbpSession.TenantId;
			}
		

            await _csAlertRepository.InsertAsync(csAlert);
         }

		 [AbpAuthorize(AppPermissions.Pages_CSAlert_Edit)]
		 protected virtual async Task Update(CreateOrEditCSAlertDto input)
         {
            var csAlert = await _csAlertRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, csAlert);
         }

		 [AbpAuthorize(AppPermissions.Pages_CSAlert_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _csAlertRepository.DeleteAsync(input.Id);
         } 
    }
}


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.AccountReceivables.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms;

namespace ERP.AccountReceivables
{
    [AbpAuthorize(AppPermissions.Pages_ARTerms)]
    public class ARTermsAppService : ERPAppServiceBase, IARTermsAppService
    {
        private readonly IRepository<ARTerm> _arTermRepository;

        private readonly IRepository<ChartofControl, string> _accountRepository;
        public ARTermsAppService(IRepository<ARTerm> arTermRepository,
             IRepository<ChartofControl, string> accountRepository)
        {
            _arTermRepository = arTermRepository;
            _accountRepository = accountRepository;

        }

        public async Task<PagedResultDto<GetARTermForViewDto>> GetAll(GetAllARTermsInput input)
        {

            var filteredARTerms = _arTermRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TERMDESC.Contains(input.Filter) || e.TERMACCID.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTermIDFilter != null, e => e.TermID >= input.MinTermIDFilter)
                        .WhereIf(input.MaxTermIDFilter != null, e => e.TermID <= input.MaxTermIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TERMDESCFilter), e => e.TERMDESC == input.TERMDESCFilter)
                        .WhereIf(input.MinTERMRATEFilter != null, e => e.TERMRATE >= input.MinTERMRATEFilter)
                        .WhereIf(input.MaxTERMRATEFilter != null, e => e.TERMRATE <= input.MaxTERMRATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TERMACCIDFilter), e => e.TERMACCID == input.TERMACCIDFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        //.WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
                        //.WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredARTerms = filteredARTerms
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var arTerms = from o in pagedAndFilteredARTerms
                          select new GetARTermForViewDto()
                          {
                              ARTerm = new ARTermDto
                              {
                                  TermId = o.TermID,
                                  TermDesc = o.TERMDESC,
                                  TermRate = o.TERMRATE,
                                  TermAccId = o.TERMACCID,
                                  Active = o.Active,
                                  AudtUser = o.AudtUser,
                                  AudtDate = o.AudtDate,
                                  CreatedBy = o.CreatedBy,
                                  CreateDate = o.CreateDate,
                                  Id = o.Id
                              }
                          };

            var totalCount = await filteredARTerms.CountAsync();

            return new PagedResultDto<GetARTermForViewDto>(
                totalCount,
                await arTerms.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ARTerms_Edit)]
        public async Task<GetARTermForEditOutput> GetARTermForEdit(EntityDto input)
        {
            var arTerm = await _arTermRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetARTermForEditOutput { ARTerm = ObjectMapper.Map<CreateOrEditARTermDto>(arTerm) };
            output.ARTerm.AccountName = _accountRepository.GetAll().Where(o => o.Id == arTerm.TERMACCID && o.TenantId == AbpSession.TenantId).Count() > 0
                ? _accountRepository.GetAll().Where(o => o.Id == arTerm.TERMACCID && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "";
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditARTermDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ARTerms_Create)]
        protected virtual async Task Create(CreateOrEditARTermDto input)
        {
            var arTerm = ObjectMapper.Map<ARTerm>(input);


            if (AbpSession.TenantId != null)
            {
                arTerm.TenantId = (int)AbpSession.TenantId;
            }


            await _arTermRepository.InsertAsync(arTerm);
        }

        [AbpAuthorize(AppPermissions.Pages_ARTerms_Edit)]
        protected virtual async Task Update(CreateOrEditARTermDto input)
        {
            var arTerm = await _arTermRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, arTerm);
        }

        [AbpAuthorize(AppPermissions.Pages_ARTerms_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _arTermRepository.DeleteAsync(input.Id);
        }
    }
}
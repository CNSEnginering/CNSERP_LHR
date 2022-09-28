using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.CaderMaster.cader_link_D.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.PayRoll.CaderMaster.cader_link_D
{
    [AbpAuthorize(AppPermissions.Pages_Cader_link_D)]
    public class Cader_link_DAppService : ERPAppServiceBase, ICader_link_DAppService
    {
        private readonly IRepository<Cader_link_D> _cader_link_DRepository;

        public Cader_link_DAppService(IRepository<Cader_link_D> cader_link_DRepository)
        {
            _cader_link_DRepository = cader_link_DRepository;

        }

        public async Task<PagedResultDto<GetCader_link_DForViewDto>> GetAll(GetAllCader_link_DInput input)
        {

            var filteredCader_link_D = _cader_link_DRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.AccountName.Contains(input.Filter) || e.Type.Contains(input.Filter) || e.PayType.Contains(input.Filter) || e.Narration.Contains(input.Filter))
                        .WhereIf(input.MinCaderIDFilter != null, e => e.CaderID >= input.MinCaderIDFilter)
                        .WhereIf(input.MaxCaderIDFilter != null, e => e.CaderID <= input.MaxCaderIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID == input.AccountIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountName == input.AccountNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type == input.TypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayTypeFilter), e => e.PayType == input.PayTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter);

            var pagedAndFilteredCader_link_D = filteredCader_link_D
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cader_link_D = from o in pagedAndFilteredCader_link_D
                               select new
                               {

                                   o.CaderID,
                                   o.AccountID,
                                   o.AccountName,
                                   o.Type,
                                   o.PayType,
                                   o.Narration,
                                   Id = o.Id
                               };

            var totalCount = await filteredCader_link_D.CountAsync();

            var dbList = await cader_link_D.ToListAsync();
            var results = new List<GetCader_link_DForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCader_link_DForViewDto()
                {
                    Cader_link_D = new Cader_link_DDto
                    {

                        CaderID = o.CaderID,
                        AccountID = o.AccountID,
                        AccountName = o.AccountName,
                        Type = o.Type,
                        PayType = o.PayType,
                        Narration = o.Narration,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCader_link_DForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCader_link_DForViewDto> GetCader_link_DForView(int id)
        {
            var cader_link_D = await _cader_link_DRepository.GetAsync(id);

            var output = new GetCader_link_DForViewDto { Cader_link_D = ObjectMapper.Map<Cader_link_DDto>(cader_link_D) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Cader_link_D_Edit)]
        public async Task<GetCader_link_DForEditOutput> GetCader_link_DForEdit(EntityDto input)
        {
            var cader_link_D = await _cader_link_DRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCader_link_DForEditOutput { Cader_link_D = ObjectMapper.Map<CreateOrEditCader_link_DDto>(cader_link_D) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCader_link_DDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Cader_link_D_Create)]
        protected virtual async Task Create(CreateOrEditCader_link_DDto input)
        {
            var cader_link_D = ObjectMapper.Map<Cader_link_D>(input);

            if (AbpSession.TenantId != null)
            {
                cader_link_D.TenantId = (int)AbpSession.TenantId;
            }

            await _cader_link_DRepository.InsertAsync(cader_link_D);

        }

        [AbpAuthorize(AppPermissions.Pages_Cader_link_D_Edit)]
        protected virtual async Task Update(CreateOrEditCader_link_DDto input)
        {
            var cader_link_D = await _cader_link_DRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cader_link_D);

        }

        [AbpAuthorize(AppPermissions.Pages_Cader_link_D_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cader_link_DRepository.DeleteAsync(input.Id);
        }

    }
}
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.CommonServices.UserLoc.CSUserLocD.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.CommonServices.UserLoc.CSUserLocD
{
    [AbpAuthorize(AppPermissions.Pages_CSUserLocD)]
    public class CSUserLocDAppService : ERPAppServiceBase, ICSUserLocDAppService
    {
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;

        public CSUserLocDAppService(IRepository<CSUserLocD> csUserLocDRepository)
        {
            _csUserLocDRepository = csUserLocDRepository;

        }

        public async Task<PagedResultDto<GetCSUserLocDForViewDto>> GetAll(GetAllCSUserLocDInput input)
        {

            var filteredCSUserLocD = _csUserLocDRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.UserID.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserIDFilter), e => e.UserID == input.UserIDFilter)
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status));

            var pagedAndFilteredCSUserLocD = filteredCSUserLocD
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var csUserLocD = from o in pagedAndFilteredCSUserLocD
                             select new
                             {

                                 o.TypeID,
                                 o.UserID,
                                 o.Status,
                                 Id = o.Id
                             };

            var totalCount = await filteredCSUserLocD.CountAsync();

            var dbList = await csUserLocD.ToListAsync();
            var results = new List<GetCSUserLocDForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCSUserLocDForViewDto()
                {
                    CSUserLocD = new CSUserLocDDto
                    {

                        TypeID = o.TypeID,
                        UserID = o.UserID,
                        Status = o.Status,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCSUserLocDForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCSUserLocDForViewDto> GetCSUserLocDForView(int id)
        {
            var csUserLocD = await _csUserLocDRepository.GetAsync(id);

            var output = new GetCSUserLocDForViewDto { CSUserLocD = ObjectMapper.Map<CSUserLocDDto>(csUserLocD) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CSUserLocD_Edit)]
        public async Task<GetCSUserLocDForEditOutput> GetCSUserLocDForEdit(EntityDto input)
        {
            var csUserLocD = await _csUserLocDRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCSUserLocDForEditOutput { CSUserLocD = ObjectMapper.Map<CreateOrEditCSUserLocDDto>(csUserLocD) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCSUserLocDDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CSUserLocD_Create)]
        protected virtual async Task Create(CreateOrEditCSUserLocDDto input)
        {
            var csUserLocD = ObjectMapper.Map<CSUserLocD>(input);

            if (AbpSession.TenantId != null)
            {
                csUserLocD.TenantId = (int)AbpSession.TenantId;
            }

            await _csUserLocDRepository.InsertAsync(csUserLocD);

        }

        [AbpAuthorize(AppPermissions.Pages_CSUserLocD_Edit)]
        protected virtual async Task Update(CreateOrEditCSUserLocDDto input)
        {
            var csUserLocD = await _csUserLocDRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, csUserLocD);

        }

        [AbpAuthorize(AppPermissions.Pages_CSUserLocD_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _csUserLocDRepository.DeleteAsync(input.Id);
        }

    }
}
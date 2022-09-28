using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.GLSLGroups.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Authorization.Users;

namespace ERP.GeneralLedger.SetupForms.GLSLGroups
{
    [AbpAuthorize(AppPermissions.Pages_GLSLGroups)]
    public class GLSLGroupsAppService : ERPAppServiceBase, IGLSLGroupsAppService
    {
        private readonly IRepository<GLSLGroups> _glslGroupsRepository;
        private readonly IRepository<User, long> _userrepository;

        public GLSLGroupsAppService(IRepository<GLSLGroups> glslGroupsRepository, IRepository<User, long> userrepository)
        {
            _glslGroupsRepository = glslGroupsRepository;
            _userrepository = userrepository;
        }

        public async Task<PagedResultDto<GetGLSLGroupsForViewDto>> GetAll(GetAllGLSLGroupsInput input)
        {

            var filteredGLSLGroups = _glslGroupsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SLGRPDESC.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinSLGrpIDFilter != null, e => e.SLGrpID >= input.MinSLGrpIDFilter)
                        .WhereIf(input.MaxSLGrpIDFilter != null, e => e.SLGrpID <= input.MaxSLGrpIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SLGRPDESCFilter), e => e.SLGRPDESC == input.SLGRPDESCFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredGLSLGroups = filteredGLSLGroups
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var glslGroups = from o in pagedAndFilteredGLSLGroups
                             select new
                             {

                                 o.SLGrpID,
                                 o.SLGRPDESC,
                                 o.Active,
                                 o.AudtUser,
                                 o.AudtDate,
                                 o.CreatedBy,
                                 o.CreateDate,
                                 Id = o.Id
                             };

            var totalCount = await filteredGLSLGroups.CountAsync();

            var dbList = await glslGroups.ToListAsync();
            var results = new List<GetGLSLGroupsForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetGLSLGroupsForViewDto()
                {
                    GLSLGroups = new GLSLGroupsDto
                    {

                        SLGrpID = o.SLGrpID,
                        SLGRPDESC = o.SLGRPDESC,
                        Active = o.Active,
                        AudtUser = o.AudtUser,
                        AudtDate = o.AudtDate,
                        CreatedBy = o.CreatedBy,
                        CreateDate = o.CreateDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetGLSLGroupsForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetGLSLGroupsForViewDto> GetGLSLGroupsForView(int id)
        {
            var glslGroups = await _glslGroupsRepository.GetAsync(id);

            var output = new GetGLSLGroupsForViewDto { GLSLGroups = ObjectMapper.Map<GLSLGroupsDto>(glslGroups) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GLSLGroups_Edit)]
        public async Task<GetGLSLGroupsForEditOutput> GetGLSLGroupsForEdit(EntityDto input)
        {
            var glslGroups = await _glslGroupsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGLSLGroupsForEditOutput { GLSLGroups = ObjectMapper.Map<CreateOrEditGLSLGroupsDto>(glslGroups) };
           
            return output;
        }
        public int? getMaxDocId()
        {
            var max = 0;
            var res = _glslGroupsRepository.GetAll().Where(v => v.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (res == null)
            {
                max = 1;
            }
            else
            {
                max = res.SLGrpID + 1;
            }
            return max;
        }

        public async Task CreateOrEdit(CreateOrEditGLSLGroupsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_GLSLGroups_Create)]
        protected virtual async Task Create(CreateOrEditGLSLGroupsDto input)
        {
            var glslGroups = ObjectMapper.Map<GLSLGroups>(input);

            if (AbpSession.TenantId != null)
            {
                glslGroups.TenantId = (int)AbpSession.TenantId;
                glslGroups.CreateDate = DateTime.Now;
                glslGroups.CreatedBy = _userrepository.GetAll().Where(v => v.TenantId == AbpSession.TenantId).FirstOrDefault().Name;
            }

            await _glslGroupsRepository.InsertAsync(glslGroups);

        }

        [AbpAuthorize(AppPermissions.Pages_GLSLGroups_Edit)]
        protected virtual async Task Update(CreateOrEditGLSLGroupsDto input)
        {
            var glslGroups = await _glslGroupsRepository.FirstOrDefaultAsync((int)input.Id);
            input.CreateDate = DateTime.Now;
            input.CreatedBy = _userrepository.GetAll().Where(v => v.TenantId == AbpSession.TenantId).FirstOrDefault().Name;
            ObjectMapper.Map(input, glslGroups);

        }

        [AbpAuthorize(AppPermissions.Pages_GLSLGroups_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _glslGroupsRepository.DeleteAsync(input.Id);
        }

    }
}
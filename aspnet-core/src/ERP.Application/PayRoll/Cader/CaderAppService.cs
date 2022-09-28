using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Cader.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Authorization.Users;

namespace ERP.PayRoll.Cader
{
    [AbpAuthorize(AppPermissions.Pages_Cader)]
    public class CaderAppService : ERPAppServiceBase, ICaderAppService
    {
        private readonly IRepository<Cader> _caderRepository;
        private readonly IRepository<User, long> _userRepository;
        public CaderAppService(IRepository<Cader> caderRepository , IRepository<User, long> userRepository)
        {
            _caderRepository = caderRepository;
            _userRepository = userRepository;

        }

        public async Task<PagedResultDto<GetCaderForViewDto>> GetAll(GetAllCaderInput input)
        {

            var filteredCader = _caderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CADER_NAME.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CADER_NAMEFilter), e => e.CADER_NAME == input.CADER_NAMEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredCader = filteredCader
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cader = from o in pagedAndFilteredCader
                        select new
                        {

                            o.CADER_NAME,
                            o.AudtUser,
                            o.AudtDate,
                            o.CreatedBy,
                            o.CreateDate,
                            Id = o.Id
                        };

            var totalCount = await filteredCader.CountAsync();

            var dbList = await cader.ToListAsync();
            var results = new List<GetCaderForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaderForViewDto()
                {
                    Cader = new CaderDto
                    {

                        CADER_NAME = o.CADER_NAME,
                        AudtUser = o.AudtUser,
                        AudtDate = o.AudtDate,
                        CreatedBy = o.CreatedBy,
                        CreateDate = o.CreateDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaderForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCaderForViewDto> GetCaderForView(int id)
        {
            var cader = await _caderRepository.GetAsync(id);

            var output = new GetCaderForViewDto { Cader = ObjectMapper.Map<CaderDto>(cader) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Cader_Edit)]
        public async Task<GetCaderForEditOutput> GetCaderForEdit(EntityDto input)
        {
            var cader = await _caderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCaderForEditOutput { Cader = ObjectMapper.Map<CreateOrEditCaderDto>(cader) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCaderDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Cader_Create)]
        protected virtual async Task Create(CreateOrEditCaderDto input)
        {
            try
            {
                var cader = ObjectMapper.Map<Cader>(input);

                if (AbpSession.TenantId != null)
                {
                    cader.TenantId = (int)AbpSession.TenantId;
                    cader.CreatedBy = _userRepository.GetAll().Where(c => c.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    cader.CreateDate = DateTime.Now;
                }

                await _caderRepository.InsertAsync(cader);
            }
            catch (Exception ex)
            {

                throw;
            }
           

        }

        [AbpAuthorize(AppPermissions.Pages_Cader_Edit)]
        protected virtual async Task Update(CreateOrEditCaderDto input)
        {
            var cader = await _caderRepository.FirstOrDefaultAsync((int)input.Id);
            cader.AudtUser = _userRepository.GetAll().Where(c => c.Id == AbpSession.UserId).SingleOrDefault().UserName;
            cader.AudtDate = DateTime.Now;
            ObjectMapper.Map(input, cader);

        }

        [AbpAuthorize(AppPermissions.Pages_Cader_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _caderRepository.DeleteAsync(input.Id);
        }

    }
}
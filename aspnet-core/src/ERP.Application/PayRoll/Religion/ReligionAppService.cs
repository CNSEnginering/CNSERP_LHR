using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Dto;
using ERP.PayRoll.Religion.Dtos;
using ERP.PayRoll.Religion.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;

namespace ERP.PayRoll.Religion
{
    [AbpAuthorize(AppPermissions.PayRoll_Religions)]
    public class ReligionAppService : ERPAppServiceBase, IReligionAppService
    {
        private readonly IRepository<Religions> _religionRepository;
        private readonly IReligionExcelExporter _religionExcelExporter;

        public ReligionAppService(IRepository<Religions> ReligionRepository, IReligionExcelExporter ReligionExcelExporter)
        {
            _religionRepository = ReligionRepository;
            _religionExcelExporter = ReligionExcelExporter;
        }

        public async Task<PagedResultDto<GetReligionForViewDto>> GetAll(GetAllReligionInput input)
        {
            var filteredReligions = _religionRepository.GetAll()
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Religion.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                .WhereIf(input.MinReligionIDFilter != null, e => e.ReligionID >= input.MinReligionIDFilter)
                                .WhereIf(input.MaxReligionIDFilter != null, e => e.ReligionID <= input.MaxReligionIDFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.ReligionFilter), e => e.Religion.ToLower() == input.ReligionFilter.ToLower().Trim())
                                .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredReligions = filteredReligions
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            var religion = from o in pagedAndFilteredReligions
                         select new GetReligionForViewDto()
                         {
                             Religion = new ReligionDto
                             {
                                 ReligionID = o.ReligionID,
                                 Religion = o.Religion,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         };
            var totalCount = await filteredReligions.CountAsync();

            return new PagedResultDto<GetReligionForViewDto>(
                totalCount,
                await religion.ToListAsync()
            );

        }

        public async Task<GetReligionForViewDto> GetReligionForView(int id)
        {
            var religion = await _religionRepository.GetAsync(id);

            var output = new GetReligionForViewDto { Religion = ObjectMapper.Map<ReligionDto>(religion) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_Religions_Edit)]
        public async Task<GetReligionForEditOutput> GetReligionForEdit(EntityDto input)
        {
            var religion = await _religionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetReligionForEditOutput { Religion = ObjectMapper.Map<CreateOrEditReligionDto>(religion) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditReligionDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_Religions_Create)]
        protected virtual async Task Create(CreateOrEditReligionDto input)
        {
            var religion = ObjectMapper.Map<Religions>(input);


            if (AbpSession.TenantId != null)
            {
                religion.TenantId = (int)AbpSession.TenantId;
            }

            religion.ReligionID = GetMaxID();
            await _religionRepository.InsertAsync(religion);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Religions_Edit)]
        protected virtual async Task Update(CreateOrEditReligionDto input)
        {
            var religion = await _religionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, religion);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Religions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _religionRepository.DeleteAsync(input.Id);
        }

       
        
        public async Task<FileDto> GetReligionToExcel(GetAllReligionForExcelInput input)
        {
            var filteredReligions = _religionRepository.GetAll()
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Religion.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                 .WhereIf(input.MinReligionIDFilter != null, e => e.ReligionID >= input.MinReligionIDFilter)
                                 .WhereIf(input.MaxReligionIDFilter != null, e => e.ReligionID <= input.MaxReligionIDFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.ReligionFilter), e => e.Religion.ToLower() == input.ReligionFilter.ToLower().Trim())
                                 .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                 .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                 .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                 .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                 .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);


            var query = (from o in filteredReligions
                         select new GetReligionForViewDto()
                         {
                             Religion = new ReligionDto
                             {
                                 ReligionID = o.ReligionID,
                                 Religion = o.Religion,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var religionListDtos = await query.ToListAsync();

            return _religionExcelExporter.ExportToFile(religionListDtos);

        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _religionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.ReligionID).Max() ?? 0) + 1;
            return maxid;
        }
    }
}

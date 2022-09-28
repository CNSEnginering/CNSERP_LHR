using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using ERP.PayRoll.Grades.Dtos;
using ERP.PayRoll.Grade.Exporting;

namespace ERP.PayRoll.Grades
{
    [AbpAuthorize(AppPermissions.PayRoll_Grades)]
    public class GradeAppService : ERPAppServiceBase, IGradeAppService
    {
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IGradeExcelExporter _gradeExcelExporter;

        public GradeAppService(IRepository<Grade> gradeRepository, IGradeExcelExporter gradeExcelExporter)
        {
            _gradeRepository = gradeRepository;
            _gradeExcelExporter = gradeExcelExporter;
        }

        public async Task<PagedResultDto<GetGradeForViewDto>> GetAll(GetAllGradeInput input)
        {
            var filteredGrades = _gradeRepository.GetAll()
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.GradeName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                .WhereIf(input.MinGradeIDFilter != null, e => e.GradeID >= input.MinGradeIDFilter)
                                .WhereIf(input.MaxGradeIDFilter != null, e => e.GradeID <= input.MaxGradeIDFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.GradeNameFilter), e => e.GradeName.ToLower() == input.GradeNameFilter.ToLower().Trim())
                                .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                .WhereIf(input.MinTypeFilter != null, e => e.Type >= input.MinTypeFilter)
                                .WhereIf(input.MaxTypeFilter != null, e => e.Type <= input.MaxTypeFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredGrades = filteredGrades
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            var grade = from o in pagedAndFilteredGrades
                         select new GetGradeForViewDto()
                         {
                             Grade = new GradeDto
                             {
                                 GradeID = o.GradeID,
                                 GradeName = o.GradeName,
                                 Type = o.Type,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         };
            var totalCount = await filteredGrades.CountAsync();

            return new PagedResultDto<GetGradeForViewDto>(
                totalCount,
                await grade.ToListAsync()
            );

        }

        public async Task<GetGradeForViewDto> GetGradeForView(int id)
        {
            var grade = await _gradeRepository.GetAsync(id);

            var output = new GetGradeForViewDto { Grade = ObjectMapper.Map<GradeDto>(grade) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_Grades_Edit)]
        public async Task<GetGradeForEditOutput> GetGradeForEdit(EntityDto input)
        {
            var grade = await _gradeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGradeForEditOutput { Grade = ObjectMapper.Map<CreateOrEditGradeDto>(grade) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGradeDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_Grades_Create)]
        protected virtual async Task Create(CreateOrEditGradeDto input)
        {
            var grade = ObjectMapper.Map<Grade>(input);


            if (AbpSession.TenantId != null)
            {
                grade.TenantId = (int)AbpSession.TenantId;
            }

            grade.GradeID = GetMaxID();
            await _gradeRepository.InsertAsync(grade);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Grades_Edit)]
        protected virtual async Task Update(CreateOrEditGradeDto input)
        {
            var grade = await _gradeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, grade);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Grades_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _gradeRepository.DeleteAsync(input.Id);
        }

       
        
        public async Task<FileDto> GetGradeToExcel(GetAllGradeForExcelInput input)
        {
            var filteredGrades = _gradeRepository.GetAll()
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.GradeName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                 .WhereIf(input.MinGradeIDFilter != null, e => e.GradeID >= input.MinGradeIDFilter)
                                 .WhereIf(input.MaxGradeIDFilter != null, e => e.GradeID <= input.MaxGradeIDFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.GradeNameFilter), e => e.GradeName.ToLower() == input.GradeNameFilter.ToLower().Trim())
                                 .WhereIf(input.MinTypeFilter != null, e => e.Type >= input.MinTypeFilter)
                                 .WhereIf(input.MaxTypeFilter != null, e => e.Type <= input.MaxTypeFilter)
                                 .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                 .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                 .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                 .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                 .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);


            var query = (from o in filteredGrades
                         select new GetGradeForViewDto()
                         {
                             Grade = new GradeDto
                             {
                                 GradeID = o.GradeID,
                                 GradeName = o.GradeName,
                                 Type = o.Type,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var gradeListDtos = await query.ToListAsync();

            return _gradeExcelExporter.ExportToFile(gradeListDtos);

        }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _gradeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.GradeID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.SalaryLock.Exporting;
using ERP.PayRoll.SalaryLock.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.PayRoll.SalaryLock
{
    [AbpAuthorize(AppPermissions.Pages_SalaryLock)]
    public class SalaryLockAppService : ERPAppServiceBase, ISalaryLockAppService
    {
        private readonly IRepository<SalaryLock> _salaryLockRepository;
        private readonly ISalaryLockExcelExporter _salaryLockExcelExporter;

        public SalaryLockAppService(IRepository<SalaryLock> salaryLockRepository, ISalaryLockExcelExporter salaryLockExcelExporter)
        {
            _salaryLockRepository = salaryLockRepository;
            _salaryLockExcelExporter = salaryLockExcelExporter;

        }

        public async Task<PagedResultDto<GetSalaryLockForViewDto>> GetAll(GetAllSalaryLockInput input)
        {

            var filteredSalaryLock = _salaryLockRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinTenantIDFilter != null, e => e.TenantId >= input.MinTenantIDFilter)
                        .WhereIf(input.MaxTenantIDFilter != null, e => e.TenantId <= input.MaxTenantIDFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth == input.MaxSalaryMonthFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear == input.MaxSalaryYearFilter)
                        .WhereIf(input.LockFilter.HasValue && input.LockFilter > -1, e => (input.LockFilter == 1 && e.Locked) || (input.LockFilter == 0 && !e.Locked))
                        .WhereIf(input.MinLockDateFilter != null, e => e.LockDate >= input.MinLockDateFilter)
                        .WhereIf(input.MaxLockDateFilter != null, e => e.LockDate <= input.MaxLockDateFilter);

            var pagedAndFilteredSalaryLock = filteredSalaryLock
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var salaryLock = from o in pagedAndFilteredSalaryLock
                             select new
                             {

                                 o.TenantId,
                                 o.SalaryMonth,
                                 o.SalaryYear,
                                 o.Locked,
                                 o.LockDate,
                                 Id = o.Id
                             };

            var totalCount = await filteredSalaryLock.CountAsync();

            var dbList = await salaryLock.ToListAsync();
            var results = new List<GetSalaryLockForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSalaryLockForViewDto()
                {
                    SalaryLock = new SalaryLockDto
                    {

                        TenantID = o.TenantId,
                        SalaryMonth = o.SalaryMonth,
                        SalaryYear = o.SalaryYear,
                        Locked = o.Locked,
                        LockDate = o.LockDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSalaryLockForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSalaryLockForViewDto> GetSalaryLockForView(int id)
        {
            var salaryLock = await _salaryLockRepository.GetAsync(id);

            var output = new GetSalaryLockForViewDto { SalaryLock = ObjectMapper.Map<SalaryLockDto>(salaryLock) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SalaryLock_Edit)]
        public async Task<GetSalaryLockForEditOutput> GetSalaryLockForEdit(EntityDto input)
        {
            var salaryLock = await _salaryLockRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalaryLockForEditOutput { SalaryLock = ObjectMapper.Map<CreateOrEditSalaryLockDto>(salaryLock) };
            output.SalaryLock.LockDate =Convert.ToDateTime(salaryLock.SalaryMonth + "/" + salaryLock.SalaryYear);
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalaryLockDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SalaryLock_Create)]
        protected virtual async Task Create(CreateOrEditSalaryLockDto input)
        {
            try
            {
                var salaryLock = ObjectMapper.Map<SalaryLock>(input);

                if (AbpSession.TenantId != null)
                {
                    salaryLock.TenantId = (int)AbpSession.TenantId;
                }

                await _salaryLockRepository.InsertAsync(salaryLock);

            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        [AbpAuthorize(AppPermissions.Pages_SalaryLock_Edit)]
        protected virtual async Task Update(CreateOrEditSalaryLockDto input)
        {
            if (AbpSession.TenantId != null)
            {
                input.TenantID = (int)AbpSession.TenantId;
            }
            var salaryLock = await _salaryLockRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, salaryLock);

        }

        [AbpAuthorize(AppPermissions.Pages_SalaryLock_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _salaryLockRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSalaryLockToExcel(GetAllSalaryLockForExcelInput input)
        {

            var filteredSalaryLock = _salaryLockRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinTenantIDFilter != null, e => e.TenantId >= input.MinTenantIDFilter)
                        .WhereIf(input.MaxTenantIDFilter != null, e => e.TenantId <= input.MaxTenantIDFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.LockFilter.HasValue && input.LockFilter > -1, e => (input.LockFilter == 1 && e.Locked) || (input.LockFilter == 0 && !e.Locked))
                        .WhereIf(input.MinLockDateFilter != null, e => e.LockDate >= input.MinLockDateFilter)
                        .WhereIf(input.MaxLockDateFilter != null, e => e.LockDate <= input.MaxLockDateFilter);

            var query = (from o in filteredSalaryLock
                         select new GetSalaryLockForViewDto()
                         {
                             SalaryLock = new SalaryLockDto
                             {
                                 TenantID = o.TenantId,
                                 SalaryMonth = o.SalaryMonth,
                                 SalaryYear = o.SalaryYear,
                                 Locked = o.Locked,
                                 LockDate = o.LockDate,
                                 Id = o.Id
                             }
                         });

            var salaryLockListDtos = await query.ToListAsync();

            return _salaryLockExcelExporter.ExportToFile(salaryLockListDtos);
        }

    }
}
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.MonthlyCPR.Exporting;
using ERP.PayRoll.MonthlyCPR.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Authorization.Users;

namespace ERP.PayRoll.MonthlyCPR
{
    [AbpAuthorize(AppPermissions.Pages_MonthlyCPR)]
    public class MonthlyCPRAppService : ERPAppServiceBase, IMonthlyCPRAppService
    {
        private readonly IRepository<MonthlyCPR> _monthlyCPRRepository;
        private readonly IMonthlyCPRExcelExporter _monthlyCPRExcelExporter;
        private readonly IRepository<SalarySheet.SalarySheet> _salarySheetRepository;
        private readonly IRepository<User, long> _userRepository;

        public MonthlyCPRAppService(IRepository<MonthlyCPR> monthlyCPRRepository, IMonthlyCPRExcelExporter monthlyCPRExcelExporter, IRepository<SalarySheet.SalarySheet> salarySheetRepository, IRepository<User, long> userRepository)
        {
            _monthlyCPRRepository = monthlyCPRRepository;
            _monthlyCPRExcelExporter = monthlyCPRExcelExporter;
            _salarySheetRepository = salarySheetRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<GetMonthlyCPRForViewDto>> GetAll(GetAllMonthlyCPRInput input)
        {
            try
            {
                var filteredMonthlyCPR = _monthlyCPRRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CPRNumber.Contains(input.Filter) || e.Remarks.Contains(input.Filter) || e.SalaryMonth.ToString().Contains(input.Filter) || e.SalaryYear.ToString().Contains(input.Filter))
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CPRNumberFilter), e => e.CPRNumber == input.CPRNumberFilter)
                        //.WhereIf(input.MinCPRDateFilter != null, e => e.CPRDate >= input.MinCPRDateFilter)
                        //.WhereIf(input.MaxCPRDateFilter != null, e => e.CPRDate <= input.MaxCPRDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .Where(e => e.TenantId == (int)AbpSession.TenantId);

                var pagedAndFilteredMonthlyCPR = filteredMonthlyCPR
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var monthlyCPR = from o in pagedAndFilteredMonthlyCPR
                                 select new
                                 {

                                     o.SalaryYear,
                                     o.SalaryMonth,
                                     o.CPRNumber,
                                     o.CPRDate,
                                     o.Amount,
                                     o.Remarks,
                                     o.Active,
                                     o.AudtUser,
                                     o.AudtDate,
                                     o.CreatedBy,
                                     o.CreateDate,
                                     Id = o.Id
                                 };

                var totalCount = await filteredMonthlyCPR.CountAsync();

                var dbList = await monthlyCPR.ToListAsync();
                var results = new List<GetMonthlyCPRForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetMonthlyCPRForViewDto()
                    {
                        MonthlyCPR = new MonthlyCPRDto
                        {

                            SalaryYear = o.SalaryYear,
                            SalaryMonth = o.SalaryMonth,
                            CPRNumber = o.CPRNumber,
                            CPRDate = o.CPRDate,
                            Amount = o.Amount,
                            Remarks = o.Remarks,
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

                return new PagedResultDto<GetMonthlyCPRForViewDto>(
                    totalCount,
                    results
                );
            }
            catch(Exception ex)
            {
                return null;
            }
            

        }

        public async Task<GetMonthlyCPRForViewDto> GetMonthlyCPRForView(int id)
        {
            var monthlyCPR = await _monthlyCPRRepository.GetAsync(id);

            var output = new GetMonthlyCPRForViewDto { MonthlyCPR = ObjectMapper.Map<MonthlyCPRDto>(monthlyCPR) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MonthlyCPR_Edit)]
        public async Task<GetMonthlyCPRForEditOutput> GetMonthlyCPRForEdit(EntityDto input)
        {
            var monthlyCPR = await _monthlyCPRRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMonthlyCPRForEditOutput { MonthlyCPR = ObjectMapper.Map<CreateOrEditMonthlyCPRDto>(monthlyCPR) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMonthlyCPRDto input)
        {
            if (input.Id == null)
            {
                try
                {
                    await Create(input);
                }
                catch(Exception ex)
                {

                }
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_MonthlyCPR_Create)]
        protected virtual async Task Create(CreateOrEditMonthlyCPRDto input)
        {
            var monthlyCPR = ObjectMapper.Map<MonthlyCPR>(input);

            if (AbpSession.TenantId != null)
            {
                monthlyCPR.TenantId = (int)AbpSession.TenantId;
            }
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            monthlyCPR.AudtUser = user;
            monthlyCPR.AudtDate = DateTime.Now;
            monthlyCPR.CreatedBy = user;
            monthlyCPR.CreateDate = DateTime.Now;
            await _monthlyCPRRepository.InsertAsync(monthlyCPR);
        }

        [AbpAuthorize(AppPermissions.Pages_MonthlyCPR_Edit)]
        protected virtual async Task Update(CreateOrEditMonthlyCPRDto input)
        {
            var monthlyCPR = await _monthlyCPRRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, monthlyCPR);

        }

        [AbpAuthorize(AppPermissions.Pages_MonthlyCPR_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _monthlyCPRRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMonthlyCPRToExcel(GetAllMonthlyCPRForExcelInput input)
        {

            var filteredMonthlyCPR = _monthlyCPRRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CPRNumber.Contains(input.Filter) || e.Remarks.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CPRNumberFilter), e => e.CPRNumber == input.CPRNumberFilter)
                        .WhereIf(input.MinCPRDateFilter != null, e => e.CPRDate >= input.MinCPRDateFilter)
                        .WhereIf(input.MaxCPRDateFilter != null, e => e.CPRDate <= input.MaxCPRDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredMonthlyCPR
                         select new GetMonthlyCPRForViewDto()
                         {
                             MonthlyCPR = new MonthlyCPRDto
                             {
                                 SalaryYear = o.SalaryYear,
                                 SalaryMonth = o.SalaryMonth,
                                 CPRNumber = o.CPRNumber,
                                 CPRDate = o.CPRDate,
                                 Amount = o.Amount,
                                 Remarks = o.Remarks,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var monthlyCPRListDtos = await query.ToListAsync();

            return _monthlyCPRExcelExporter.ExportToFile(monthlyCPRListDtos);
        }

        public double? GetTotalTaxAmount(int SalaryYear, int SalaryMonth)
        {
            var taxAmount = _salarySheetRepository.GetAll().Where(e => e.SalaryMonth == SalaryMonth && e.SalaryYear == SalaryYear && e.TenantId == AbpSession.TenantId).Sum(x=>x.tax_amount);
            return taxAmount;
        }

    }
}
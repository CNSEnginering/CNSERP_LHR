

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Allowances.Exporting;
using ERP.PayRoll.Allowances.Dtos;
using ERP.PayRoll.SalarySheet;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.Allowances
{
    [AbpAuthorize(AppPermissions.PayRoll_Allowances)]
    public class AllowancesAppService : ERPAppServiceBase, IAllowancesAppService
    {
        private readonly IRepository<Allowances> _allowancesRepository;
        private readonly IRepository<AllowancesDetail> _allowancesDetailRepository;
        private readonly IAllowancesDetailsAppService _allowancesDetailsAppService;
        private readonly IAllowancesExcelExporter _allowancesExcelExporter;
        private readonly IRepository<Employees.Employees> _employeesRepository;
        private readonly IRepository<AllowanceSetup.AllowanceSetup> _allowanceSetupRepository;
        private readonly IRepository<SalarySheet.SalarySheet> _salarySheetRepository;

        public AllowancesAppService(IRepository<Allowances> allowancesRepository, IRepository<AllowancesDetail> allowancesDetailRepository,
            IAllowancesDetailsAppService allowancesDetailsAppService, IAllowancesExcelExporter allowancesExcelExporter,
            IRepository<Employees.Employees> employeesRepository, IRepository<AllowanceSetup.AllowanceSetup> allowanceSetupRepository, IRepository<SalarySheet.SalarySheet> salarySheetRepository)
        {
            _salarySheetRepository = salarySheetRepository;
            _allowancesRepository = allowancesRepository;
            _allowancesDetailRepository = allowancesDetailRepository;
            _allowancesDetailsAppService = allowancesDetailsAppService;
            _allowancesExcelExporter = allowancesExcelExporter;
            _employeesRepository = employeesRepository;
            _allowanceSetupRepository = allowanceSetupRepository;
        }

        public async Task<PagedResultDto<GetAllowancesForViewDto>> GetAll(GetAllAllowancesInput input)
        {

            var filteredAllowances = _allowancesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocIDFilter != null, e => e.DocID >= input.MinDocIDFilter)
                        .WhereIf(input.MaxDocIDFilter != null, e => e.DocID <= input.MaxDocIDFilter)
                        .WhereIf(input.MinDocdateFilter != null, e => e.Docdate >= input.MinDocdateFilter)
                        .WhereIf(input.MaxDocdateFilter != null, e => e.Docdate <= input.MaxDocdateFilter)
                        .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                        .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                        .WhereIf(input.MinDocYearFilter != null, e => e.DocYear >= input.MinDocYearFilter)
                        .WhereIf(input.MaxDocYearFilter != null, e => e.DocYear <= input.MaxDocYearFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredAllowances = filteredAllowances.Where(x=>x.TenantId==AbpSession.TenantId)
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var allowances = from o in pagedAndFilteredAllowances
                             select new GetAllowancesForViewDto()
                             {
                                 Allowances = new AllowancesDto
                                 {
                                     DocID = o.DocID,
                                     Docdate = o.Docdate,
                                     DocMonth = o.DocMonth,
                                     DocYear = o.DocYear,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredAllowances.CountAsync();

            return new PagedResultDto<GetAllowancesForViewDto>(
                totalCount,
                await allowances.ToListAsync()
            );
        }

        public async Task<GetAllowancesForViewDto> GetAllowancesForView(int id)
        {
            var allowances = await _allowancesRepository.GetAsync(id);

            var output = new GetAllowancesForViewDto { Allowances = ObjectMapper.Map<AllowancesDto>(allowances) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_Allowances_Edit)]
        public async Task<GetAllowancesForEditOutput> GetAllowancesForEdit(EntityDto input)
        {
            var allowances = await _allowancesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAllowancesForEditOutput { Allowances = ObjectMapper.Map<CreateOrEditAllowancesDto>(allowances) };

            var allowanceDetail = await _allowancesDetailsAppService.GetAllowancesDetailForEdit((int)output.Allowances.Id);
            output.Allowances.AllowancesDetail = allowanceDetail.AllowancesDetail;

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAllowancesDto input)
            {
            if (!input.flag)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }

        }

        [AbpAuthorize(AppPermissions.PayRoll_Allowances_Create)]
        protected virtual async Task Create(CreateOrEditAllowancesDto input)
        {
            try
            {
                var checkMonth = CheckvalidMonth(int.Parse(input.DocMonth.ToString()), int.Parse(input.DocYear.ToString()));
                //if (checkMonth==0)
                //{
                var allowances = ObjectMapper.Map<Allowances>(input);


                if (AbpSession.TenantId != null)
                {
                    allowances.TenantId = (int)AbpSession.TenantId;
                }
                allowances.DocID = GetMaxID();
                int Hid = await _allowancesRepository.InsertAndGetIdAsync(allowances);

                foreach (var item in input.AllowancesDetail)
                {
                    item.DetID = Hid;
                }

                await _allowancesDetailsAppService.CreateOrEdit(input.AllowancesDetail);

                //}
                //else
                //{

                //}

            }
            catch (Exception ex)
            {

            }

        }

        public int CheckvalidMonth(int Month, int Year)
        {
            try
            {
                var IsExist = _allowancesRepository.GetAll().Where(x => x.DocMonth == Month && x.DocYear == Year).FirstOrDefaultAsync();
                if (IsExist != null)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {

                return -1;
            }
        }
        [AbpAuthorize(AppPermissions.PayRoll_Allowances_Edit)]
        protected virtual async Task Update(CreateOrEditAllowancesDto input)
        {
            var allowances = await _allowancesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, allowances);

            foreach (var item in input.AllowancesDetail)
            {
                item.DetID = (int)input.Id;
            }

            await _allowancesDetailsAppService.Delete((int)input.Id);
            await _allowancesDetailsAppService.CreateOrEdit(input.AllowancesDetail);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Allowances_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _allowancesRepository.DeleteAsync(input.Id);
            await _allowancesDetailsAppService.Delete(input.Id);
        }

        public async Task<FileDto> GetAllowancesToExcel(GetAllAllowancesForExcelInput input)
        {

            var filteredAllowances = _allowancesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocIDFilter != null, e => e.DocID >= input.MinDocIDFilter)
                        .WhereIf(input.MaxDocIDFilter != null, e => e.DocID <= input.MaxDocIDFilter)
                        .WhereIf(input.MinDocdateFilter != null, e => e.Docdate >= input.MinDocdateFilter)
                        .WhereIf(input.MaxDocdateFilter != null, e => e.Docdate <= input.MaxDocdateFilter)
                        .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                        .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                        .WhereIf(input.MinDocYearFilter != null, e => e.DocYear >= input.MinDocYearFilter)
                        .WhereIf(input.MaxDocYearFilter != null, e => e.DocYear <= input.MaxDocYearFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredAllowances
                         select new GetAllowancesForViewDto()
                         {
                             Allowances = new AllowancesDto
                             {
                                 DocID = o.DocID,
                                 Docdate = o.Docdate,
                                 DocMonth = o.DocMonth,
                                 DocYear = o.DocYear,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var allowancesListDtos = await query.ToListAsync();

            return _allowancesExcelExporter.ExportToFile(allowancesListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _allowancesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocID).Max() ?? 0) + 1;
            return maxid;
        }

        public bool? GetValidMonthYear(string MM,string YY)
        {
            var Data = _allowancesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocMonth == int.Parse(MM) && o.DocYear == int.Parse(YY)).FirstOrDefault();
            return Data==null?false:true;
        }

        public async Task<PagedResultDto<AllowancesDetailDto>> GetCarAllowanceData(string DocMonth,string DocYear)
        {
            int month = 0;
            int year = 0;
            var DaysOfMonth = 0;
            if (DocMonth != null && DocMonth != "" && DocYear != null && DocYear != "")
            {
                month = Convert.ToInt32(DocMonth);
                year = Convert.ToInt32(DocYear);
                DaysOfMonth = DateTime.DaysInMonth(year, month);
            }

            var employees = _employeesRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Active == true && e.AllowanceType != 0);
            var salarySheet = _salarySheetRepository.GetAll().Where(e => e.SalaryMonth == month && e.SalaryYear == year && e.TenantId == AbpSession.TenantId);

            double? fuelRate = _allowanceSetupRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId).OrderByDescending(x => x.DocID).First().FuelRate;
            double? PerMilageRate = _allowanceSetupRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId).OrderByDescending(x => x.DocID).First().PerLiterMilage;
            double? RepairRate = _allowanceSetupRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId).OrderByDescending(x => x.DocID).First().RepairRate;
            var allowancesDetails = from o in employees
                                    join s in salarySheet on o.EmployeeID equals s.EmployeeID into ss
                                    from s in ss.DefaultIfEmpty()
                                    select new AllowancesDetailDto
                                    {
                                        EmployeeID = o.EmployeeID,
                                        EmployeeName = o.EmployeeName,
                                        AllowanceType = o.AllowanceType,
                                        WorkedDays = s.work_days ?? 0,
                                        AllowanceTypeName = (o.AllowanceType == 1) ? "Car" : (o.AllowanceType == 2) ? "Motorcycle" : "",
                                        AllowanceAmt = o.AllowanceAmt ?? 0,
                                        AllowanceQty = o.AllowanceQty ?? 0,
                                        Amount = (o.AllowanceType == 1) ? ((o.AllowanceAmt > 0) ? ((o.AllowanceAmt / DaysOfMonth) * Convert.ToInt32((s.work_days ?? 0))) : (((o.AllowanceQty / DaysOfMonth) * Convert.ToInt32((s.work_days ?? 0))) * fuelRate)) : 0,
                                        PerlitrMilg = (o.AllowanceType == 1) ? 0 : PerMilageRate,
                                        RepairRate = (o.AllowanceType == 1) ? 0 : (o.AllowanceType == 2) ? RepairRate : 0,

                                    };
            var totalCount = await allowancesDetails.CountAsync();
            var test = allowancesDetails.Where(x => x.AllowanceType != null).ToListAsync();
            return new PagedResultDto<AllowancesDetailDto>(
                totalCount,
                await allowancesDetails.Where(x => x.AllowanceType != null).ToListAsync()
            );
        }

    }
}


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Adjustments.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.PayRoll.EmployeeDeductions;
using ERP.PayRoll.EmployeeDeductions.Dtos;
using ERP.PayRoll.EmployeeEarnings;

namespace ERP.PayRoll.Adjustments
{
    [AbpAuthorize(AppPermissions.Pages_AdjH)]
    public class AdjHAppService : ERPAppServiceBase, IAdjHAppService
    {
        private readonly IRepository<AdjH> _adjHRepository;
        private readonly IEmployeeDeductionsAppService _EmployeeDeductionsAppService;
        private readonly IEmployeeEarningsAppService _EmployeeEarningsAppService;

        public AdjHAppService(IRepository<AdjH> adjHRepository, IEmployeeDeductionsAppService employeeDeductionsAppService,
            IEmployeeEarningsAppService employeeEarningsAppService
            )
        {
            _adjHRepository = adjHRepository;
            _EmployeeDeductionsAppService = employeeDeductionsAppService;
            _EmployeeEarningsAppService = employeeEarningsAppService;

        }

        public async Task<PagedResultDto<GetAdjHForViewDto>> GetAll(GetAllAdjHInput input)
        {

            var filteredAdjH = _adjHRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || input.Filter.ToLower() == "deduction" ? e.DocType == 2 : e.DocType == 1 || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTenantIDFilter != null, e => e.TenantId >= input.MinTenantIDFilter)
                        .WhereIf(input.MaxTenantIDFilter != null, e => e.TenantId <= input.MaxTenantIDFilter)
                        .WhereIf(input.MinDocTypeFilter != null, e => e.DocType >= input.MinDocTypeFilter)
                        .WhereIf(input.MaxDocTypeFilter != null, e => e.DocType == input.MaxDocTypeFilter)
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinDocIDFilter != null, e => e.DocID >= input.MinDocIDFilter)
                        .WhereIf(input.MaxDocIDFilter != null, e => e.DocID <= input.MaxDocIDFilter)
                        .WhereIf(input.MinDocdateFilter != null, e => e.Docdate >= input.MinDocdateFilter)
                        .WhereIf(input.MaxDocdateFilter != null, e => e.Docdate <= input.MaxDocdateFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredAdjH = filteredAdjH
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var adjH = from o in pagedAndFilteredAdjH
                       select new GetAdjHForViewDto()
                       {
                           AdjH = new AdjHDto
                           {
                               TenantID = o.TenantId,
                               DocType = o.DocType,
                               TypeID = o.TypeID,
                               DocID = o.DocID,
                               Docdate = o.Docdate,
                               SalaryYear = o.SalaryYear,
                               SalaryMonth = o.SalaryMonth,
                               AudtUser = o.AudtUser,
                               AudtDate = o.AudtDate,
                               CreatedBy = o.CreatedBy,
                               CreateDate = o.CreateDate,
                               Id = o.Id
                           }
                       };

            var totalCount = await filteredAdjH.CountAsync();

            return new PagedResultDto<GetAdjHForViewDto>(
                totalCount,
                await adjH.ToListAsync()
            );
        }

        public async Task<GetAdjHForViewDto> GetAdjHForView(int id)
        {
            var adjH = await _adjHRepository.GetAsync(id);

            var output = new GetAdjHForViewDto { AdjH = ObjectMapper.Map<AdjHDto>(adjH) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AdjH_Edit)]
        public async Task<GetAdjHForEditOutput> GetAdjHForEdit(EntityDto input)
        {
            var adjH = await _adjHRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAdjHForEditOutput { AdjH = ObjectMapper.Map<CreateOrEditAdjHDto>(adjH) };

            output.AdjH.AdjDetails = new AdjTypeDetail();
            if (output.AdjH.DocType == 1)
            {
                var adjDetails = await _EmployeeEarningsAppService.GetEmployeeEarningsForEdit((int)output.AdjH.Id);
                output.AdjH.AdjDetails.EarningDetail = adjDetails.EmployeeEarnings;
            }
            else
            {
                var adjDetails = await _EmployeeDeductionsAppService.GetEmployeeDeductionsForEdit((int)output.AdjH.Id);
                output.AdjH.AdjDetails.DeductionDetail = adjDetails.EmployeeDeductions;
            }


            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAdjHDto input)
        {
            if (input.Id == null || input.Id==-1)
            {
                input.Id = null;
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AdjH_Create)]
        protected virtual async Task Create(CreateOrEditAdjHDto input)
        {
            var adjH = ObjectMapper.Map<AdjH>(input);


            if (AbpSession.TenantId != null)
            {
                adjH.TenantId = (int)AbpSession.TenantId;
            }


            var Hid = await _adjHRepository.InsertAndGetIdAsync(adjH);

            if (input.DocType == 1)
            {

                foreach (var item in input.AdjDetails.EarningDetail)
                {
                    item.Detid = Hid;
                }

                await _EmployeeEarningsAppService.CreateOrEdit(input.AdjDetails.EarningDetail);
            }

            if (input.DocType == 2)
            {

                foreach (var item in input.AdjDetails.DeductionDetail)
                {
                    item.Detid = Hid;
                }

                await _EmployeeDeductionsAppService.CreateOrEdit(input.AdjDetails.DeductionDetail);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AdjH_Edit)]
        protected virtual async Task Update(CreateOrEditAdjHDto input)
        {
            var adjH = await _adjHRepository.FirstOrDefaultAsync((int)input.Id);
            input.TenantID = adjH.TenantId;
            ObjectMapper.Map(input, adjH);

            if (input.DocType == 1)
            {
                foreach (var item in input.AdjDetails.EarningDetail)
                {
                    item.Detid = (int)input.Id;
                }

                await _EmployeeEarningsAppService.Delete((int)input.Id);
                await _EmployeeEarningsAppService.CreateOrEdit(input.AdjDetails.EarningDetail);
            }

            if (input.DocType == 2)
            {
                foreach (var item in input.AdjDetails.DeductionDetail)
                {
                    item.Detid = (int)input.Id;
                }

                await _EmployeeDeductionsAppService.Delete((int)input.Id);
                await _EmployeeDeductionsAppService.CreateOrEdit(input.AdjDetails.DeductionDetail);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AdjH_Delete)]
        public async Task Delete(int Id, int DocType)
        {
            await _adjHRepository.DeleteAsync(Id);

            if (DocType == 1)
            {
                await _EmployeeEarningsAppService.Delete(Id);
            }

            if (DocType == 2)
            {
                await _EmployeeDeductionsAppService.Delete(Id);
            }
        }

        public int GetMaxDocID()
        {
            int? DocID = _adjHRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)?.Max(x => x.DocID) ?? 0;

            return DocID == null ? 1 : Convert.ToInt32(DocID + 1);
        }
    }
}
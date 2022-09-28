using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.hrmSetup.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.GeneralLedger.SetupForms;
using Microsoft.AspNetCore.Mvc;
using ERP.Authorization.Users;

namespace ERP.PayRoll.hrmSetup
{
    [AbpAuthorize(AppPermissions.Pages_HrmSetup)]
    public class HrmSetupAppService : ERPAppServiceBase, IHrmSetupAppService
    {
        private readonly IRepository<HrmSetup> _hrmSetupRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<User, long> _userrepository;

        public HrmSetupAppService(IRepository<HrmSetup> hrmSetupRepository, IRepository<User, long> userrepository, IRepository<ChartofControl, string> chartofControlRepository)
        {
            _hrmSetupRepository = hrmSetupRepository;
            _chartofControlRepository = chartofControlRepository;
            _userrepository = userrepository;

        }
        [HttpGet]
        public HrmSetupDto GetsetupForLoad()
        {
            var data = _hrmSetupRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId).Select(v => new HrmSetupDto()
            {
                AdvToPayable = v.AdvToPayable,
                Id = v.Id,
                AdvToStSal = v.AdvToStSal,
                LoanToPayable = v.LoanToPayable,
                LoanToStSal = v.LoanToStSal
            }).FirstOrDefault();
            if (data != null)
            {
                data.LoanToPayableName = _chartofControlRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.Id == data.LoanToPayable).FirstOrDefault().AccountName;
                data.LoanToStSalName = _chartofControlRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.Id == data.LoanToStSal).FirstOrDefault().AccountName;
                data.AdvToPayablName = _chartofControlRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.Id == data.AdvToPayable).FirstOrDefault().AccountName;
                data.AdvToStSalName = _chartofControlRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.Id == data.AdvToStSal).FirstOrDefault().AccountName;
            }

            return data;
        }


        public async Task<PagedResultDto<GetHrmSetupForViewDto>> GetAll(GetAllHrmSetupInput input)
        {

            var filteredHrmSetup = _hrmSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AdvToStSal.Contains(input.Filter) || e.AdvToPayable.Contains(input.Filter) || e.LoanToStSal.Contains(input.Filter) || e.LoanToPayable.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AuditBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdvToStSalFilter), e => e.AdvToStSal == input.AdvToStSalFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdvToPayableFilter), e => e.AdvToPayable == input.AdvToPayableFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LoanToStSalFilter), e => e.LoanToStSal == input.LoanToStSalFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LoanToPayableFilter), e => e.LoanToPayable == input.LoanToPayableFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(input.MinAuditDateFilter != null, e => e.AuditDate >= input.MinAuditDateFilter)
                        .WhereIf(input.MaxAuditDateFilter != null, e => e.AuditDate <= input.MaxAuditDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuditByFilter), e => e.AuditBy == input.AuditByFilter);

            var pagedAndFilteredHrmSetup = filteredHrmSetup
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hrmSetup = from o in pagedAndFilteredHrmSetup
                           select new
                           {

                               o.AdvToStSal,
                               o.AdvToPayable,
                               o.LoanToStSal,
                               o.LoanToPayable,
                               o.CreatedBy,
                               o.CreateDate,
                               o.AuditDate,
                               o.AuditBy,
                               Id = o.Id
                           };

            var totalCount = await filteredHrmSetup.CountAsync();

            var dbList = await hrmSetup.ToListAsync();
            var results = new List<GetHrmSetupForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHrmSetupForViewDto()
                {
                    HrmSetup = new HrmSetupDto
                    {

                        AdvToStSal = o.AdvToStSal,
                        AdvToPayable = o.AdvToPayable,
                        LoanToStSal = o.LoanToStSal,
                        LoanToPayable = o.LoanToPayable,
                        CreatedBy = o.CreatedBy,
                        CreateDate = o.CreateDate,
                        AuditDate = o.AuditDate,
                        AuditBy = o.AuditBy,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHrmSetupForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_HrmSetup_Edit)]
        public async Task<GetHrmSetupForEditOutput> GetHrmSetupForEdit(EntityDto input)
        {
            var hrmSetup = await _hrmSetupRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHrmSetupForEditOutput { HrmSetup = ObjectMapper.Map<CreateOrEditHrmSetupDto>(hrmSetup) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHrmSetupDto input)
        {
            Delete();
            await Create(input);
        }

        //public async Task CreateOrEdit(CreateOrEditHrmSetupDto input)
        //{
        //    if (input.Id == null)
        //    {
        //        await Create(input);
        //    }
        //    else
        //    {
        //        await Update(input);
        //    }
        //}

        [AbpAuthorize(AppPermissions.Pages_HrmSetup_Create)]
        protected virtual async Task Create(CreateOrEditHrmSetupDto input)
        {
            var hrmSetup = ObjectMapper.Map<HrmSetup>(input);

            if (AbpSession.TenantId != null)
            {
                hrmSetup.TenantId = (int)AbpSession.TenantId;
                hrmSetup.AuditDate = DateTime.Now;
                hrmSetup.CreateDate = DateTime.Now;
                hrmSetup.CreatedBy = _userrepository.GetAll().Where(c => c.Id == AbpSession.UserId && c.TenantId == AbpSession.TenantId).FirstOrDefault().UserName;
                hrmSetup.AuditBy = _userrepository.GetAll().Where(c => c.Id == AbpSession.UserId && c.TenantId == AbpSession.TenantId).FirstOrDefault().UserName;
            }

            await _hrmSetupRepository.InsertAsync(hrmSetup);

        }

        //[AbpAuthorize(AppPermissions.Pages_HrmSetup_Create)]
        //protected virtual async Task Create(CreateOrEditHrmSetupDto input)
        //{
        //    var hrmSetup = ObjectMapper.Map<HrmSetup>(input);

        //    if (AbpSession.TenantId != null)
        //    {
        //        hrmSetup.TenantId = (int)AbpSession.TenantId;
        //    }

        //    await _hrmSetupRepository.InsertAsync(hrmSetup);

        //}

        [AbpAuthorize(AppPermissions.Pages_HrmSetup_Edit)]
        protected virtual async Task Update(CreateOrEditHrmSetupDto input)
        {
            var hrmSetup = await _hrmSetupRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, hrmSetup);

        }

        [AbpAuthorize(AppPermissions.Pages_HrmSetup_Delete)]
        public async Task Delete()
        {
            await _hrmSetupRepository.DeleteAsync(c => c.TenantId == AbpSession.TenantId);
        }

        //[AbpAuthorize(AppPermissions.Pages_HrmSetup_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //    await _hrmSetupRepository.DeleteAsync(input.Id);
        //}

    }
}
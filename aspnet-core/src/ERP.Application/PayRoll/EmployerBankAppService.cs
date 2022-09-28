

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll
{
    [AbpAuthorize(AppPermissions.Pages_EmployerBank)]
    public class EmployerBankAppService : ERPAppServiceBase, IEmployerBankAppService
    {
        private readonly IRepository<EmployerBank> _employerBankRepository;


        public EmployerBankAppService(IRepository<EmployerBank> employerBankRepository)
        {
            _employerBankRepository = employerBankRepository;

        }

        public async Task<PagedResultDto<GetEmployerBankForViewDto>> GetAll(GetAllEmployerBankInput input)
        {

            var filteredEmployerBank = _employerBankRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EBankName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEBankIDFilter != null, e => e.EBankID >= input.MinEBankIDFilter)
                        .WhereIf(input.MaxEBankIDFilter != null, e => e.EBankID <= input.MaxEBankIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EBankNameFilter), e => e.EBankName == input.EBankNameFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredEmployerBank = filteredEmployerBank
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employerBank = from o in pagedAndFilteredEmployerBank
                               select new GetEmployerBankForViewDto()
                               {
                                   EmployerBank = new EmployerBankDto
                                   {
                                       EBankID = o.EBankID,
                                       EBankName = o.EBankName,
                                       Active = o.Active,
                                       AudtUser = o.AudtUser,
                                       AudtDate = o.AudtDate,
                                       CreatedBy = o.CreatedBy,
                                       CreateDate = o.CreateDate,
                                       Id = o.Id
                                   }
                               };

            var totalCount = await filteredEmployerBank.CountAsync();

            return new PagedResultDto<GetEmployerBankForViewDto>(
                totalCount,
                await employerBank.ToListAsync()
            );
        }

        public async Task<GetEmployerBankForViewDto> GetEmployerBankForView(int id)
        {
            var employerBank = await _employerBankRepository.GetAsync(id);

            var output = new GetEmployerBankForViewDto { EmployerBank = ObjectMapper.Map<EmployerBankDto>(employerBank) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_EmployerBank_Edit)]
        public async Task<GetEmployerBankForEditOutput> GetEmployerBankForEdit(EntityDto input)
        {
            var employerBank = await _employerBankRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployerBankForEditOutput { EmployerBank = ObjectMapper.Map<CreateOrEditEmployerBankDto>(employerBank) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployerBankDto input)
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

        [AbpAuthorize(AppPermissions.Pages_EmployerBank_Create)]
        protected virtual async Task Create(CreateOrEditEmployerBankDto input)
        {
            var employerBank = ObjectMapper.Map<EmployerBank>(input);


            if (AbpSession.TenantId != null)
            {
                employerBank.TenantId = (int)AbpSession.TenantId;
            }


            await _employerBankRepository.InsertAsync(employerBank);
        }

        [AbpAuthorize(AppPermissions.Pages_EmployerBank_Edit)]
        protected virtual async Task Update(CreateOrEditEmployerBankDto input)
        {
            var employerBank = await _employerBankRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employerBank);
        }

        [AbpAuthorize(AppPermissions.Pages_EmployerBank_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _employerBankRepository.DeleteAsync(input.Id);
        }

        public async Task<List<EmployerBankDto>> GetEmployerBanks()
        {
            var bankList = _employerBankRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)?.
                Select(x => new EmployerBankDto
                {
                    EBankID = x.EBankID,
                    EBankName = x.EBankName
                });



            return await bankList.ToListAsync();


        }

    }
}
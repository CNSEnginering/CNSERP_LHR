

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms.LCExpenses;
using ERP.GeneralLedger.SetupForms.LCExpenses.Dtos;
using ERP.GeneralLedger.SetupForms.LCExpenses.Exporting;

namespace ERP.GeneralLedger.SetupForms.LCExpenses
{
    [AbpAuthorize(AppPermissions.Pages_LCExpenses)]
    public class LCExpensesAppService : ERPAppServiceBase, ILCExpensesAppService
    {
        private readonly IRepository<LCExpenses> _lcExpensesRepository;
        private readonly ILCExpensesExcelExporter _lcExpensesExcelExporter;

        public LCExpensesAppService(
            IRepository<LCExpenses> LCExpensesRepository, 
            ILCExpensesExcelExporter LCExpensesExcelExporter)
        {
            _lcExpensesRepository = LCExpensesRepository;
            _lcExpensesExcelExporter = LCExpensesExcelExporter;
        }

        public async Task<PagedResultDto<GetLCExpensesForViewDto>> GetAll(GetAllLCExpensesInput input)
        {
            var filteredExpenses = _lcExpensesRepository.GetAll()
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ExpDesc.Contains(input.Filter) || e.AuditUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                   .WhereIf(input.MinExpIDFilter != null, e => e.ExpID >= input.MinExpIDFilter)
                                   .WhereIf(input.MaxExpIDFilter != null, e => e.ExpID <= input.MaxExpIDFilter)
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.ExpDescFilter), e => e.ExpDesc.ToLower() == input.ExpDescFilter.ToLower().Trim())
                                   .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                                   .WhereIf(input.MinAuditDateFilter != null, e => e.AuditDate >= input.MinAuditDateFilter)
                                   .WhereIf(input.MaxAuditDateFilter != null, e => e.AuditDate <= input.MaxAuditDateFilter)
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                   .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                   .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredLCExpensess = filteredExpenses
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            var lcExpense = from o in pagedAndFilteredLCExpensess
                      select new GetLCExpensesForViewDto()
                      {
                          LCExpenses = new LCExpensesDto
                          {
                              ExpID = o.ExpID,
                              ExpDesc = o.ExpDesc,
                              Active = o.Active,
                              AuditUser = o.AuditUser,
                              AuditDate = o.AuditDate,
                              CreatedBy = o.CreatedBy,
                              CreateDate = o.CreateDate,
                              Id = o.Id
                          }
                      };
            var totalCount = await filteredExpenses.CountAsync();

            return new PagedResultDto<GetLCExpensesForViewDto>(
                totalCount,
                await lcExpense.ToListAsync()
            );
        }
        public async Task<GetLCExpensesForViewDto> GetLCExpensesForView(int id)
        {
            var lcExpense = await _lcExpensesRepository.GetAsync(id);

            var output = new GetLCExpensesForViewDto { LCExpenses = ObjectMapper.Map<LCExpensesDto>(lcExpense) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpenses_Edit)]
        public async Task<GetLCExpensesForEditOutput> GetLCExpensesForEdit(EntityDto input)
        {
            var lcExpense = await _lcExpensesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLCExpensesForEditOutput { LCExpenses = ObjectMapper.Map<CreateOrEditLCExpensesDto>(lcExpense) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpenses_Create)]
        public async Task CreateOrEdit(CreateOrEditLCExpensesDto input)
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
        [AbpAuthorize(AppPermissions.Pages_LCExpenses_Create)]
        protected virtual async Task Create(CreateOrEditLCExpensesDto input)
        {
            var lcExpense = ObjectMapper.Map<LCExpenses>(input);


            if (AbpSession.TenantId != null)
            {
                lcExpense.TenantId = (int)AbpSession.TenantId;
            }

            lcExpense.ExpID = GetMaxID();
            await _lcExpensesRepository.InsertAsync(lcExpense);
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpenses_Edit)]
        protected virtual async Task Update(CreateOrEditLCExpensesDto input)
        {
            var lcExpense = await _lcExpensesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, lcExpense);
        }
        [AbpAuthorize(AppPermissions.Pages_LCExpenses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _lcExpensesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLCExpensesToExcel(GetAllLCExpensesForExcelInput input)
        {
            var filteredExpenses = _lcExpensesRepository.GetAll()
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ExpDesc.Contains(input.Filter) || e.AuditUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                   .WhereIf(input.MinExpIDFilter != null, e => e.ExpID >= input.MinExpIDFilter)
                                   .WhereIf(input.MaxExpIDFilter != null, e => e.ExpID <= input.MaxExpIDFilter)
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.ExpDescFilter), e => e.ExpDesc.ToLower() == input.ExpDescFilter.ToLower().Trim())
                                   .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                                   .WhereIf(input.MinAuditDateFilter != null, e => e.AuditDate >= input.MinAuditDateFilter)
                                   .WhereIf(input.MaxAuditDateFilter != null, e => e.AuditDate <= input.MaxAuditDateFilter)
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                   .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                   .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = from o in filteredExpenses
                            select new GetLCExpensesForViewDto()
                            {
                                LCExpenses = new LCExpensesDto
                                {
                                    ExpID = o.ExpID,
                                    ExpDesc = o.ExpDesc,
                                    Active = o.Active,
                                    AuditUser = o.AuditUser,
                                    AuditDate = o.AuditDate,
                                    CreatedBy = o.CreatedBy,
                                    CreateDate = o.CreateDate,
                                    Id = o.Id
                                }
                            };

            var lcExpenseListDtos = await query.ToListAsync();

            return _lcExpensesExcelExporter.ExportToFile(lcExpenseListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _lcExpensesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.ExpID).Max() ?? 0) + 1;
            return maxid;
        }


    }
}
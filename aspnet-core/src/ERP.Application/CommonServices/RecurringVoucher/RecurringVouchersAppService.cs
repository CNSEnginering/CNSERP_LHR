

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.CommonServices.RecurringVoucher.Exporting;
using ERP.CommonServices.RecurringVoucher.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.CommonServices.RecurringVoucher
{
    [AbpAuthorize(AppPermissions.Pages_RecurringVouchers)]
    public class RecurringVouchersAppService : ERPAppServiceBase, IRecurringVouchersAppService
    {
        private readonly IRepository<RecurringVoucher> _recurringVoucherRepository;
        private readonly IRecurringVouchersExcelExporter _recurringVouchersExcelExporter;


        public RecurringVouchersAppService(IRepository<RecurringVoucher> recurringVoucherRepository, IRecurringVouchersExcelExporter recurringVouchersExcelExporter)
        {
            _recurringVoucherRepository = recurringVoucherRepository;
            _recurringVouchersExcelExporter = recurringVouchersExcelExporter;

        }

        public async Task<PagedResultDto<GetRecurringVoucherForViewDto>> GetAll(GetAllRecurringVouchersInput input)
        {

            var filteredRecurringVouchers = _recurringVoucherRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.FmtVoucherNo.Contains(input.Filter) || e.Reference.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID == input.BookIDFilter)
                        .WhereIf(input.MinVoucherNoFilter != null, e => e.VoucherNo >= input.MinVoucherNoFilter)
                        .WhereIf(input.MaxVoucherNoFilter != null, e => e.VoucherNo <= input.MaxVoucherNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FmtVoucherNoFilter), e => e.FmtVoucherNo == input.FmtVoucherNoFilter)
                        .WhereIf(input.MinVoucherDateFilter != null, e => e.VoucherDate >= input.MinVoucherDateFilter)
                        .WhereIf(input.MaxVoucherDateFilter != null, e => e.VoucherDate <= input.MaxVoucherDateFilter)
                        .WhereIf(input.MinVoucherMonthFilter != null, e => e.VoucherMonth >= input.MinVoucherMonthFilter)
                        .WhereIf(input.MaxVoucherMonthFilter != null, e => e.VoucherMonth <= input.MaxVoucherMonthFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceFilter), e => e.Reference == input.ReferenceFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredRecurringVouchers = filteredRecurringVouchers
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var recurringVouchers = from o in pagedAndFilteredRecurringVouchers
                                    select new GetRecurringVoucherForViewDto()
                                    {
                                        RecurringVoucher = new RecurringVoucherDto
                                        {
                                            DocNo = o.DocNo,
                                            BookID = o.BookID,
                                            VoucherNo = o.VoucherNo,
                                            FmtVoucherNo = o.FmtVoucherNo,
                                            VoucherDate = o.VoucherDate,
                                            VoucherMonth = o.VoucherMonth,
                                            ConfigID = o.ConfigID,
                                            Reference = o.Reference,
                                            Active = o.Active,
                                            AudtUser = o.AudtUser,
                                            AudtDate = o.AudtDate,
                                            CreatedBy = o.CreatedBy,
                                            CreateDate = o.CreateDate,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredRecurringVouchers.CountAsync();

            return new PagedResultDto<GetRecurringVoucherForViewDto>(
                totalCount,
                await recurringVouchers.ToListAsync()
            );
        }

        public async Task<GetRecurringVoucherForViewDto> GetRecurringVoucherForView(int id)
        {
            var recurringVoucher = await _recurringVoucherRepository.GetAsync(id);

            var output = new GetRecurringVoucherForViewDto { RecurringVoucher = ObjectMapper.Map<RecurringVoucherDto>(recurringVoucher) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RecurringVouchers_Edit)]
        public async Task<GetRecurringVoucherForEditOutput> GetRecurringVoucherForEdit(EntityDto input)
        {
            var recurringVoucher = await _recurringVoucherRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRecurringVoucherForEditOutput { RecurringVoucher = ObjectMapper.Map<CreateOrEditRecurringVoucherDto>(recurringVoucher) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRecurringVoucherDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RecurringVouchers_Create)]
        protected virtual async Task Create(CreateOrEditRecurringVoucherDto input)
        {
            var recurringVoucher = ObjectMapper.Map<RecurringVoucher>(input);


            if (AbpSession.TenantId != null)
            {
                recurringVoucher.TenantId = (int)AbpSession.TenantId;
            }

            recurringVoucher.DocNo = GetMaxID();

            await _recurringVoucherRepository.InsertAsync(recurringVoucher);
        }

        [AbpAuthorize(AppPermissions.Pages_RecurringVouchers_Edit)]
        protected virtual async Task Update(CreateOrEditRecurringVoucherDto input)
        {
            var recurringVoucher = await _recurringVoucherRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, recurringVoucher);
        }

        [AbpAuthorize(AppPermissions.Pages_RecurringVouchers_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _recurringVoucherRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRecurringVouchersToExcel(GetAllRecurringVouchersForExcelInput input)
        {

            var filteredRecurringVouchers = _recurringVoucherRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.FmtVoucherNo.Contains(input.Filter) || e.Reference.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID == input.BookIDFilter)
                        .WhereIf(input.MinVoucherNoFilter != null, e => e.VoucherNo >= input.MinVoucherNoFilter)
                        .WhereIf(input.MaxVoucherNoFilter != null, e => e.VoucherNo <= input.MaxVoucherNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FmtVoucherNoFilter), e => e.FmtVoucherNo == input.FmtVoucherNoFilter)
                        .WhereIf(input.MinVoucherDateFilter != null, e => e.VoucherDate >= input.MinVoucherDateFilter)
                        .WhereIf(input.MaxVoucherDateFilter != null, e => e.VoucherDate <= input.MaxVoucherDateFilter)
                        .WhereIf(input.MinVoucherMonthFilter != null, e => e.VoucherMonth >= input.MinVoucherMonthFilter)
                        .WhereIf(input.MaxVoucherMonthFilter != null, e => e.VoucherMonth <= input.MaxVoucherMonthFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceFilter), e => e.Reference == input.ReferenceFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredRecurringVouchers
                         select new GetRecurringVoucherForViewDto()
                         {
                             RecurringVoucher = new RecurringVoucherDto
                             {
                                 DocNo = o.DocNo,
                                 BookID = o.BookID,
                                 VoucherNo = o.VoucherNo,
                                 FmtVoucherNo = o.FmtVoucherNo,
                                 VoucherDate = o.VoucherDate,
                                 VoucherMonth = o.VoucherMonth,
                                 ConfigID = o.ConfigID,
                                 Reference = o.Reference,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var recurringVoucherListDtos = await query.ToListAsync();

            return _recurringVouchersExcelExporter.ExportToFile(recurringVoucherListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _recurringVoucherRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            return maxid;
        }

    }
}


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.BankReconcile.Exporting;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.CommonServices;
using Abp.Collections.Extensions;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;

namespace ERP.GeneralLedger.Transaction.BankReconcile
{
    [AbpAuthorize(AppPermissions.Pages_BankReconcileDetails)]
    public class BankReconcileDetailsAppService : ERPAppServiceBase, IBankReconcileDetailsAppService
    {
        private readonly IRepository<BankReconcileDetail> _bankReconcileDetailRepository;
        private readonly IRepository<BankReconcile> _bankReconcileHeaderRepository;
        private readonly IBankReconcileDetailsExcelExporter _bankReconcileDetailsExcelExporter;
        private readonly IRepository<AccountsPosting> _accountsPostingRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;

        private readonly IRepository<Bank> _bankRepository;


        public BankReconcileDetailsAppService(IRepository<BankReconcileDetail> bankReconcileDetailRepository, IBankReconcileDetailsExcelExporter bankReconcileDetailsExcelExporter
            , IRepository<AccountsPosting> accountsPostingRepository
            , IRepository<Bank> bankRepository,
            IRepository<BankReconcile> bankReconcileHeaderRepository,
            IRepository<GLTRHeader> gltrHeaderRepository)
        {
            _bankReconcileDetailRepository = bankReconcileDetailRepository;
            _bankReconcileDetailsExcelExporter = bankReconcileDetailsExcelExporter;
            _accountsPostingRepository = accountsPostingRepository;
            _bankReconcileHeaderRepository = bankReconcileHeaderRepository;
            _bankRepository = bankRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
        }

        public async Task<PagedResultDto<GetBankReconcileDetailForViewDto>> GetAll(GetAllBankReconcileDetailsInput input)
        {

            var filteredBankReconcileDetails = _bankReconcileDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID == input.BookIDFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(input.MinVoucherIDFilter != null, e => e.VoucherID >= input.MinVoucherIDFilter)
                        .WhereIf(input.MaxVoucherIDFilter != null, e => e.VoucherID <= input.MaxVoucherIDFilter)
                        .WhereIf(input.MinVoucherDateFilter != null, e => e.VoucherDate >= input.MinVoucherDateFilter)
                        .WhereIf(input.MaxVoucherDateFilter != null, e => e.VoucherDate <= input.MaxVoucherDateFilter)
                        .WhereIf(input.MinClearingDateFilter != null, e => e.ClearingDate >= input.MinClearingDateFilter)
                        .WhereIf(input.MaxClearingDateFilter != null, e => e.ClearingDate <= input.MaxClearingDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.IncludeFilter > -1, e => (input.IncludeFilter == 1 && e.Include) || (input.IncludeFilter == 0 && !e.Include));

            var pagedAndFilteredBankReconcileDetails = filteredBankReconcileDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var bankReconcileDetails = from o in pagedAndFilteredBankReconcileDetails
                                       select new GetBankReconcileDetailForViewDto()
                                       {
                                           BankReconcileDetail = new BankReconcileDetailDto
                                           {
                                               DetID = o.DetID,
                                               BookID = o.BookID,
                                               ConfigID = o.ConfigID,
                                               VoucherID = o.VoucherID,
                                               VoucherDate = o.VoucherDate,
                                               ClearingDate = o.ClearingDate,
                                               Amount = o.Amount,
                                               Include = o.Include,
                                               Id = o.Id
                                           }
                                       };

            var totalCount = await filteredBankReconcileDetails.CountAsync();

            return new PagedResultDto<GetBankReconcileDetailForViewDto>(
                totalCount,
                await bankReconcileDetails.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BankReconcileDetails_Edit)]
        public async Task<GetBankReconcileDetailForEditOutput> GetBankReconcileDetailForEdit(int ID)
        {

            var bankReconcileDetail = await _bankReconcileDetailRepository.GetAllListAsync(x => x.DetID == ID && x.TenantId == AbpSession.TenantId);
            var gLTRHeaders = await _gltrHeaderRepository.GetAllListAsync(x => x.TenantId == AbpSession.TenantId);

            var bankReconcileDetails = from o in bankReconcileDetail
                                       join h in gLTRHeaders on o.GLDetID equals h.Id
                                       select new CreateOrEditBankReconcileDetailDto
                                       {
                                           DetID = o.DetID,
                                           BookID = o.BookID,
                                           ConfigID = o.BookID + '-' + o.ConfigID,
                                           VoucherID = o.VoucherID,
                                           VoucherDate = o.VoucherDate,
                                           ClearingDate = o.ClearingDate,
                                           Dr = o.Amount > 0 ? o.Amount : 0,
                                           Cr = o.Amount < 0 ? o.Amount : 0,
                                           Include = o.Include,
                                           GLDetID = o.GLDetID,
                                           Id = o.Id,
                                           ChNumber = h.ChNumber

                                       };



            var output = new GetBankReconcileDetailForEditOutput { BankReconcileDetail = ObjectMapper.Map<ICollection<CreateOrEditBankReconcileDetailDto>>(bankReconcileDetails) };
            return output;
        }

        public async Task CreateOrEdit(ICollection<CreateOrEditBankReconcileDetailDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    await Create(item);
                }
                else
                {
                    await Update(item);
                }
            }

        }

        [AbpAuthorize(AppPermissions.Pages_BankReconcileDetails_Create)]
        protected virtual async Task Create(CreateOrEditBankReconcileDetailDto input)
        {
            // var bankReconcileDetail = ObjectMapper.Map<BankReconcileDetail>(input);

            string[] config = input.ConfigID.Split('-');

            BankReconcileDetail bankReconcileDetail = new BankReconcileDetail();
            bankReconcileDetail.BookID = input.BookID;
            bankReconcileDetail.ClearingDate = input.ClearingDate;
            bankReconcileDetail.ConfigID = Convert.ToInt32(config[1]);
            bankReconcileDetail.DetID = input.DetID;
            bankReconcileDetail.Include = input.Include;
            bankReconcileDetail.VoucherDate = input.VoucherDate;
            bankReconcileDetail.VoucherID = input.VoucherID;
            bankReconcileDetail.GLDetID = input.GLDetID;

            bankReconcileDetail.Amount = input.Dr - input.Cr;


            if (AbpSession.TenantId != null)
            {
                bankReconcileDetail.TenantId = (int)AbpSession.TenantId;
            }


            await _bankReconcileDetailRepository.InsertAsync(bankReconcileDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_BankReconcileDetails_Edit)]
        protected virtual async Task Update(CreateOrEditBankReconcileDetailDto input)
        {

            string[] config = input.ConfigID.Split('-');
            BankReconcileDetail bankReconcileDetail = new BankReconcileDetail();
            bankReconcileDetail.BookID = input.BookID;
            bankReconcileDetail.ClearingDate = input.ClearingDate;
            bankReconcileDetail.ConfigID = Convert.ToInt32(config[1]);
            bankReconcileDetail.DetID = input.DetID;
            bankReconcileDetail.Include = input.Include;
            bankReconcileDetail.VoucherDate = input.VoucherDate;
            bankReconcileDetail.VoucherID = input.VoucherID;
            bankReconcileDetail.GLDetID = input.GLDetID;

            bankReconcileDetail.Amount = input.Dr - input.Cr;
            await _bankReconcileDetailRepository.InsertAsync(bankReconcileDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_BankReconcileDetails_Delete)]
        public async Task Delete(int input)
        {
            await _bankReconcileDetailRepository.DeleteAsync(x => x.DetID == input);
        }

        public async Task<FileDto> GetBankReconcileDetailsToExcel(GetAllBankReconcileDetailsForExcelInput input)
        {

            var filteredBankReconcileDetails = _bankReconcileDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID == input.BookIDFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(input.MinVoucherIDFilter != null, e => e.VoucherID >= input.MinVoucherIDFilter)
                        .WhereIf(input.MaxVoucherIDFilter != null, e => e.VoucherID <= input.MaxVoucherIDFilter)
                        .WhereIf(input.MinVoucherDateFilter != null, e => e.VoucherDate >= input.MinVoucherDateFilter)
                        .WhereIf(input.MaxVoucherDateFilter != null, e => e.VoucherDate <= input.MaxVoucherDateFilter)
                        .WhereIf(input.MinClearingDateFilter != null, e => e.ClearingDate >= input.MinClearingDateFilter)
                        .WhereIf(input.MaxClearingDateFilter != null, e => e.ClearingDate <= input.MaxClearingDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.IncludeFilter > -1, e => (input.IncludeFilter == 1 && e.Include) || (input.IncludeFilter == 0 && !e.Include));

            var query = (from o in filteredBankReconcileDetails
                         select new GetBankReconcileDetailForViewDto()
                         {
                             BankReconcileDetail = new BankReconcileDetailDto
                             {
                                 DetID = o.DetID,
                                 BookID = o.BookID,
                                 ConfigID = o.ConfigID,
                                 VoucherID = o.VoucherID,
                                 VoucherDate = o.VoucherDate,
                                 ClearingDate = o.ClearingDate,
                                 Amount = o.Amount,
                                 Include = o.Include,
                                 Id = o.Id
                             }
                         });


            var bankReconcileDetailListDtos = await query.ToListAsync();

            return _bankReconcileDetailsExcelExporter.ExportToFile(bankReconcileDetailListDtos);
        }

        public async Task<ListResultDto<CreateOrEditBankReconcileDetailDto>> GetListOFdetail(string bank, string date)
        {
            var reconsileDate = Convert.ToDateTime(date);

            var recondet = (from a in _bankReconcileDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                            join
                            b in _bankReconcileHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                            on new { A = a.DetID, B = a.TenantId } equals new { A = b.Id, B = b.TenantId }
                            where (b.TenantId == AbpSession.TenantId && a.Include == true && b.BankID == bank)
                            select (a.GLDetID)
                           ).ToList();

            var bankAcc = _bankRepository.FirstOrDefault(x => x.BANKID == bank && x.TenantId == AbpSession.TenantId).IDACCTBANK;
            var filteredBankReconcileDetails = _accountsPostingRepository.GetAll().Where(x => x.AccountID == bankAcc && x.TenantId == AbpSession.TenantId
            && x.DocDate.Date <= reconsileDate.Date && x.Approved != false);

            var bankReconcileDetails = from o in filteredBankReconcileDetails
                                       where !recondet.Contains(o.Id)

                                       //orderby o.DocNo ascending

                                       //orderby o.FmtDocNo ascending
                                       //orderby o.Amount descending

                                       select new CreateOrEditBankReconcileDetailDto
                                       {
                                           GLDetID = o.Id,
                                           ConfigID = o.BookID + "-" + o.ConfigID.ToString(),
                                           BookID = o.BookID,
                                           VoucherID = o.DocNo,
                                           FmtDocNo = o.FmtDocNo,
                                           VoucherDate = o.DocDate,
                                           ClearingDate = reconsileDate,
                                           Dr = o.Amount > 0 ? o.Amount : 0,
                                           Cr = o.Amount < 0 ? Math.Abs((double)o.Amount) : 0,
                                           Include = false,
                                           ChNumber = o.ChNumber

                                       };
            return new ListResultDto<CreateOrEditBankReconcileDetailDto>(
                await bankReconcileDetails.OrderByDescending(x => x.Dr).ThenBy(x => x.FmtDocNo).ToListAsync()
                );
        }

    }
}
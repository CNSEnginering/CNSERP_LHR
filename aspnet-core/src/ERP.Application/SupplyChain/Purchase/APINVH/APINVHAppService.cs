using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Purchase.APINVH.Exporting;
using ERP.SupplyChain.Purchase.APINVH.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;

namespace ERP.SupplyChain.Purchase.APINVH
{
    [AbpAuthorize(AppPermissions.Pages_APINVH)]
    public class APINVHAppService : ERPAppServiceBase, IAPINVHAppService
    {
        private readonly IRepository<APINVH> _apinvhRepository;
        private readonly IAPINVHExcelExporter _apinvhExcelExporter;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLOption> _glOptionRepository;
        private readonly IRepository<GLSecChartofControl, string> _glSecChartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;

        public APINVHAppService(IRepository<User, long> userRepository, IRepository<GLTRDetail> gltrDetailRepository, IRepository<GLOption> glOptionRepository, IRepository<GLTRHeader> gltrHeaderRepository, IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<GLSecChartofControl, string> glSecChartofControlRepository, IRepository<APINVH> apinvhRepository, IAPINVHExcelExporter apinvhExcelExporter, IRepository<TaxAuthority, string> taxAuthorityRepository)
        {
            _apinvhRepository = apinvhRepository;
            _apinvhExcelExporter = apinvhExcelExporter;
            _gltrDetailRepository = gltrDetailRepository;
            _userRepository = userRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
            _glOptionRepository = glOptionRepository;
            _glSecChartofControlRepository = glSecChartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _taxAuthorityRepository = taxAuthorityRepository;
        }
       
        public async Task<PagedResultDto<GetAPINVHForViewDto>> GetAll(GetAllAPINVHInput input)
        {

            var filteredAPINVH = _apinvhRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VAccountID.Contains(input.Filter) || e.PartyInvNo.Contains(input.Filter) || e.PaymentOption.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.BAccountID.Contains(input.Filter) || e.ChequeNo.Contains(input.Filter) || e.CurID.Contains(input.Filter) || e.TaxAuth.Contains(input.Filter) || e.TaxAccID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.RefNo.Contains(input.Filter) || e.PayReason.Contains(input.Filter) || e.PostedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.DocStatus.Contains(input.Filter) || e.CprNo.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VAccountIDFilter), e => e.VAccountID == input.VAccountIDFilter)
                        .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartyInvNoFilter), e => e.PartyInvNo == input.PartyInvNoFilter)
                        .WhereIf(input.MinPartyInvDateFilter != null, e => e.PartyInvDate >= input.MinPartyInvDateFilter)
                        .WhereIf(input.MaxPartyInvDateFilter != null, e => e.PartyInvDate <= input.MaxPartyInvDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDiscAmountFilter != null, e => e.DiscAmount >= input.MinDiscAmountFilter)
                        .WhereIf(input.MaxDiscAmountFilter != null, e => e.DiscAmount <= input.MaxDiscAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentOptionFilter), e => e.PaymentOption == input.PaymentOptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID == input.BankIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BAccountIDFilter), e => e.BAccountID == input.BAccountIDFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChequeNoFilter), e => e.ChequeNo == input.ChequeNoFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.ChTypeFilter), e => e.ChType == input.ChTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurIDFilter), e => e.CurID == input.CurIDFilter)
                        .WhereIf(input.MinCurRateFilter != null, e => e.CurRate >= input.MinCurRateFilter)
                        .WhereIf(input.MaxCurRateFilter != null, e => e.CurRate <= input.MaxCurRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuthFilter), e => e.TaxAuth == input.TaxAuthFilter)
                        .WhereIf(input.MinTaxClassFilter != null, e => e.TaxClass >= input.MinTaxClassFilter)
                        .WhereIf(input.MaxTaxClassFilter != null, e => e.TaxClass <= input.MaxTaxClassFilter)
                        .WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        .WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAccIDFilter), e => e.TaxAccID == input.TaxAccIDFilter)
                        .WhereIf(input.MinTaxAmountFilter != null, e => e.TaxAmount >= input.MinTaxAmountFilter)
                        .WhereIf(input.MaxTaxAmountFilter != null, e => e.TaxAmount <= input.MaxTaxAmountFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinPostDateFilter != null, e => e.PostDate >= input.MinPostDateFilter)
                        .WhereIf(input.MaxPostDateFilter != null, e => e.PostDate <= input.MaxPostDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RefNoFilter), e => e.RefNo == input.RefNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayReasonFilter), e => e.PayReason == input.PayReasonFilter)
                      //  .WhereIf(input.PostedFilter.HasValue && input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostedByFilter), e => e.PostedBy == input.PostedByFilter)
                        .WhereIf(input.MinPostedDateFilter != null, e => e.PostedDate >= input.MinPostedDateFilter)
                        .WhereIf(input.MaxPostedDateFilter != null, e => e.PostedDate <= input.MaxPostedDateFilter)
                        .WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
                        .WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocStatusFilter), e => e.DocStatus == input.DocStatusFilter)
                        .WhereIf(input.MinCprIDFilter != null, e => e.CprID >= input.MinCprIDFilter)
                        .WhereIf(input.MaxCprIDFilter != null, e => e.CprID <= input.MaxCprIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CprNoFilter), e => e.CprNo == input.CprNoFilter)
                        .WhereIf(input.MinCprDateFilter != null, e => e.CprDate >= input.MinCprDateFilter)
                        .WhereIf(input.MaxCprDateFilter != null, e => e.CprDate <= input.MaxCprDateFilter);

            var pagedAndFilteredAPINVH = filteredAPINVH
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var apinvh = from o in pagedAndFilteredAPINVH
                         select new GetAPINVHForViewDto()
                         {
                             APINVH = new APINVHDto
                             {
                                 DocNo = o.DocNo,
                                 VAccountID = o.VAccountID,
                                 SubAccID = o.SubAccID,
                                 PartyInvNo = o.PartyInvNo,
                                 PartyInvDate = o.PartyInvDate,
                                 Amount = o.Amount,
                                 DiscAmount = o.DiscAmount,
                                 PaymentOption = o.PaymentOption,
                                 BankID = o.BankID,
                                 BAccountID = o.BAccountID,
                                 ConfigID = o.ConfigID,
                                 ChequeNo = o.ChequeNo,
                                 ChType = o.ChType,
                                 CurID = o.CurID,
                                 CurRate = o.CurRate,
                                 TaxAuth = o.TaxAuth,
                                 TaxClass = o.TaxClass,
                                 TaxRate = o.TaxRate,
                                 TaxAccID = o.TaxAccID,
                                 TaxAmount = o.TaxAmount,
                                 DocDate = o.DocDate,
                                 PostDate = o.PostDate,
                                 Narration = o.Narration,
                                 RefNo = o.RefNo,
                                 PayReason = o.PayReason,
                                 Posted = o.Posted,
                                 Approve=o.Approved,
                                 PostedBy = o.PostedBy,
                                 PostedDate = o.PostedDate,
                                 LinkDetID = o.LinkDetID,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 DocStatus = o.DocStatus,
                                 CprID = o.CprID,
                                 CprNo = o.CprNo,
                                 CprDate = o.CprDate,
                                 Id = o.Id
                             }
                         };

            var totalCount = await filteredAPINVH.CountAsync();

            return new PagedResultDto<GetAPINVHForViewDto>(
                totalCount,
                await apinvh.ToListAsync()
            );
        }

        public async Task<GetAPINVHForViewDto> GetAPINVHForView(int id)
        {
            var apinvh = await _apinvhRepository.GetAsync(id);

            var output = new GetAPINVHForViewDto { APINVH = ObjectMapper.Map<APINVHDto>(apinvh) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_APINVH_Edit)]
        public async Task<GetAPINVHForEditOutput> GetAPINVHForEdit(EntityDto input)
        {
            try
            {
                var apinvh = await _apinvhRepository.FirstOrDefaultAsync(input.Id);
                var taxAuthorityName = _taxAuthorityRepository.GetAll().Where(x => x.Id == apinvh.TaxAuth).FirstOrDefault().TAXAUTHDESC;
                var accountName = _glSecChartofControlRepository.GetAll().Where(x => x.Id == apinvh.VAccountID).FirstOrDefault().AccountName;
                var SubaccountName = _accountSubLedgerRepository.GetAll().Where(x => x.Id == apinvh.SubAccID && x.AccountID == apinvh.VAccountID).FirstOrDefault().SubAccName;
                var output = new GetAPINVHForEditOutput { APINVH = ObjectMapper.Map<CreateOrEditAPINVHDto>(apinvh) };
                output.APINVH.TaxAccName = taxAuthorityName;
                output.APINVH.SubAccName = SubaccountName;
                output.APINVH.VAccountName = accountName;
                output.APINVH.PendingAmt =apinvh.Amount- (_apinvhRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.PartyInvNo == apinvh.PartyInvNo).Sum(x => x.PaidAmt)) + apinvh.PaidAmt;
                return output;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public void ApprovalData(int[] postedData, string Mode, bool bit)
        {
            try
            {
                var postedDataIds = postedData.Distinct();
                if (Mode == "UnApproval")
                {
                    (from a in _apinvhRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.Id))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = false;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                     });
                }
                else
                {
                    (from a in _apinvhRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.Id))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                     });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ProcessApinvh(CreateOrEditAPINVHDto input)
        {
            //input.GLTRHeader.Posted = true;
            //input.GLTRHeader.PostedBy = input.GLTRHeader.CreatedBy;
            //input.GLTRHeader.PostedDate = DateTime.Now;

          

            (from a in _apinvhRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id==input.Id)
             select a).ToList().ForEach(x =>
             {
                 x.Posted = true;
                 x.PostedDate = DateTime.Now;
                 x.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
             });

            var apinvhData = ObjectMapper.Map<APINVH>(input);

            GLTRHeader gltrHeader = new GLTRHeader();

            if (AbpSession.TenantId != null)
            {
                gltrHeader.TenantId = (int)AbpSession.TenantId;
            }
            gltrHeader.DocDate =Convert.ToDateTime(apinvhData.DocDate);
            gltrHeader.BookID = "BP";
            gltrHeader.ConfigID = 1;
            gltrHeader.DocNo = Convert.ToInt32(GetMaxDocId(gltrHeader.BookID, false, null));
            gltrHeader.FmtDocNo = Convert.ToInt32(GetMaxDocId(gltrHeader.BookID, true, gltrHeader.DocDate));
            gltrHeader.DocMonth = gltrHeader.DocDate.Month;
            gltrHeader.NARRATION = "Receipt Inovice Payment has Done";
            gltrHeader.Posted = false;
            gltrHeader.Approved = true;
            gltrHeader.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            gltrHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            gltrHeader.CreatedOn = DateTime.Now;
            gltrHeader.AprovedDate = DateTime.Now;
            gltrHeader.CURID = apinvhData.CurID;
            gltrHeader.CURRATE = apinvhData.CurRate;
            gltrHeader.LocId = 1;//Convert.ToInt32(apinvhData.LocId);

            var getGenratedId = _gltrHeaderRepository.InsertAndGetId(gltrHeader);
            var message = "";
            if (getGenratedId!=null)
            {
                if (input.VAccountID!=null && input.VAccountID!="")
                {
                    GLTRDetail GlDetail = new GLTRDetail()
                    {
                        DetID = getGenratedId,
                        AccountID = input.VAccountID,
                        SubAccID = input.SubAccID,
                        Narration = "Amount Paid By " + input.SubAccName,
                        Amount = input.PaidAmt,
                        LocId =1,// Convert.ToInt32(input.LocId),
                        TenantId = Convert.ToInt32(AbpSession.TenantId),
                        IsAuto = false
                    };
                  var gldata= ObjectMapper.Map<GLTRDetail>(GlDetail);
                    _gltrDetailRepository.InsertAsync(gldata);
                }
                else
                {
                    message = "NoAccount";
                }
                if (input.BAccountID != null && input.BAccountID != "")
                {
                    GLTRDetail GlDetail = new GLTRDetail()
                    {
                        DetID = getGenratedId,
                        AccountID = input.BAccountID,
                        SubAccID = 0,
                        ChequeNo = input.ChequeNo,
                        Narration = "Amount Paid By " + input.SubAccName,
                        Amount = -input.PaidAmt,
                        LocId = 1,// Convert.ToInt32(input.LocId),
                        TenantId = Convert.ToInt32(AbpSession.TenantId),
                        IsAuto = true
                    };
                    var gldata2 = ObjectMapper.Map<GLTRDetail>(GlDetail);
                    _gltrDetailRepository.InsertAsync(gldata2);
                    message = "Save";
                }
                else
                {
                    message = "NoAccount";
                }
            }

           

            
            return message;
        }

        public int GetMaxDocId(string bookId, bool fmtDocNoRequired, DateTime? docDate)
        {
            int maxid = 0;
            int fmtDocNo = 0;
            //DateTime docDate = DateTime.Now;
            int? docFrequency = 0;
            if (bookId != "")
            {
                maxid = ((from tab1 in _gltrHeaderRepository.GetAll().Where(o => o.BookID == bookId && o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            }

            if (fmtDocNoRequired == true)
            {
                var glOptionsData = _glOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                if (glOptionsData.Count() > 0)
                {
                    docFrequency = glOptionsData.FirstOrDefault().DocFrequency;
                    if (docFrequency == 1)
                    {
                        fmtDocNo = maxid;
                    }
                    else if (docFrequency == 2)
                    {
                        var maxfmtDocId = ((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.DocDate.Year ==
                        docDate.Value.Year
                        )
                                                /// select (int?)a.DocNo).Max() ?? 0) + 1;
                                            select (int?)Convert.ToInt32(a.FmtDocNo)).Max() ?? 0) + 1;
                        fmtDocNo = maxfmtDocId;
                        //fmtDocNo = maxfmtDocId.ToString("D6");
                        //fmtDocNo = (bookId + "-" + fmtDocNo);
                    }
                    else if (docFrequency == 3)
                    {
                        var maxfmtDocId = ((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.DocDate.Year ==
                        docDate.Value.Year && o.DocDate.Month == docDate.Value.Month
                        )
                                            select (int?)Convert.ToInt32(a.FmtDocNo)).Max() ?? 0) + 1;
                        //fmtDocNo = maxfmtDocId.ToString("D6");
                        //fmtDocNo = (bookId + "-" + fmtDocNo);
                        fmtDocNo = maxfmtDocId;
                    }
                }
            }

            return fmtDocNoRequired == true ? fmtDocNo : maxid;
        }


        public async Task CreateOrEdit(CreateOrEditAPINVHDto input)
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

        [AbpAuthorize(AppPermissions.Pages_APINVH_Create)]
        protected virtual async Task Create(CreateOrEditAPINVHDto input)
        {

            var apinvh = ObjectMapper.Map<APINVH>(input);

            if (AbpSession.TenantId != null)
            {
                apinvh.TenantId = (int)AbpSession.TenantId;
                apinvh.CreateDate = DateTime.Now;
                apinvh.PartyInvDate = apinvh.PartyInvDate.Value.AddDays(1);
                apinvh.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            }

            await _apinvhRepository.InsertAsync(apinvh);
        }

        public int GetmaxDocNo()
        {
            var tenID = 0;
            if (AbpSession.TenantId != 0)
            {
                tenID = (int)AbpSession.TenantId;
            }
            var DocNo = _apinvhRepository.GetAll().Where(x => x.TenantId == tenID).DefaultIfEmpty().Max(x => x.DocNo);
            return DocNo=DocNo+1;
         
        }
        [AbpAuthorize(AppPermissions.Pages_APINVH_Edit)]
        protected virtual async Task Update(CreateOrEditAPINVHDto input)
        {
            //input.DocDate = input.DocDate.Value.AddDays(1);
            //input.PartyInvDate= input.PartyInvDate.Value.AddDays(1);
            var apinvh = await _apinvhRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtDate = DateTime.Now;
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.CreateDate = apinvh.CreateDate;
            ObjectMapper.Map(input, apinvh);
        }

        [AbpAuthorize(AppPermissions.Pages_APINVH_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _apinvhRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAPINVHToExcel(GetAllAPINVHForExcelInput input)
        {

            var filteredAPINVH = _apinvhRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.VAccountID.Contains(input.Filter) || e.PartyInvNo.Contains(input.Filter) || e.PaymentOption.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.BAccountID.Contains(input.Filter) || e.ChequeNo.Contains(input.Filter) || e.CurID.Contains(input.Filter) || e.TaxAuth.Contains(input.Filter) || e.TaxAccID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.RefNo.Contains(input.Filter) || e.PayReason.Contains(input.Filter) || e.PostedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.DocStatus.Contains(input.Filter) || e.CprNo.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VAccountIDFilter), e => e.VAccountID == input.VAccountIDFilter)
                        .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartyInvNoFilter), e => e.PartyInvNo == input.PartyInvNoFilter)
                        .WhereIf(input.MinPartyInvDateFilter != null, e => e.PartyInvDate >= input.MinPartyInvDateFilter)
                        .WhereIf(input.MaxPartyInvDateFilter != null, e => e.PartyInvDate <= input.MaxPartyInvDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDiscAmountFilter != null, e => e.DiscAmount >= input.MinDiscAmountFilter)
                        .WhereIf(input.MaxDiscAmountFilter != null, e => e.DiscAmount <= input.MaxDiscAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentOptionFilter), e => e.PaymentOption == input.PaymentOptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID == input.BankIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BAccountIDFilter), e => e.BAccountID == input.BAccountIDFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChequeNoFilter), e => e.ChequeNo == input.ChequeNoFilter)
                      //  .WhereIf(!string.IsNullOrWhiteSpace(input.ChTypeFilter), e => e.ChType == input.ChTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurIDFilter), e => e.CurID == input.CurIDFilter)
                        .WhereIf(input.MinCurRateFilter != null, e => e.CurRate >= input.MinCurRateFilter)
                        .WhereIf(input.MaxCurRateFilter != null, e => e.CurRate <= input.MaxCurRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuthFilter), e => e.TaxAuth == input.TaxAuthFilter)
                        .WhereIf(input.MinTaxClassFilter != null, e => e.TaxClass >= input.MinTaxClassFilter)
                        .WhereIf(input.MaxTaxClassFilter != null, e => e.TaxClass <= input.MaxTaxClassFilter)
                        .WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        .WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAccIDFilter), e => e.TaxAccID == input.TaxAccIDFilter)
                        .WhereIf(input.MinTaxAmountFilter != null, e => e.TaxAmount >= input.MinTaxAmountFilter)
                        .WhereIf(input.MaxTaxAmountFilter != null, e => e.TaxAmount <= input.MaxTaxAmountFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinPostDateFilter != null, e => e.PostDate >= input.MinPostDateFilter)
                        .WhereIf(input.MaxPostDateFilter != null, e => e.PostDate <= input.MaxPostDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RefNoFilter), e => e.RefNo == input.RefNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayReasonFilter), e => e.PayReason == input.PayReasonFilter)
                       // .WhereIf(input.PostedFilter.HasValue && input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostedByFilter), e => e.PostedBy == input.PostedByFilter)
                        .WhereIf(input.MinPostedDateFilter != null, e => e.PostedDate >= input.MinPostedDateFilter)
                        .WhereIf(input.MaxPostedDateFilter != null, e => e.PostedDate <= input.MaxPostedDateFilter)
                        .WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
                        .WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocStatusFilter), e => e.DocStatus == input.DocStatusFilter)
                        .WhereIf(input.MinCprIDFilter != null, e => e.CprID >= input.MinCprIDFilter)
                        .WhereIf(input.MaxCprIDFilter != null, e => e.CprID <= input.MaxCprIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CprNoFilter), e => e.CprNo == input.CprNoFilter)
                        .WhereIf(input.MinCprDateFilter != null, e => e.CprDate >= input.MinCprDateFilter)
                        .WhereIf(input.MaxCprDateFilter != null, e => e.CprDate <= input.MaxCprDateFilter);

            var query = (from o in filteredAPINVH
                         select new GetAPINVHForViewDto()
                         {
                             APINVH = new APINVHDto
                             {
                                 DocNo = o.DocNo,
                                 VAccountID = o.VAccountID,
                                 SubAccID = o.SubAccID,
                                 PartyInvNo = o.PartyInvNo,
                                 PartyInvDate = o.PartyInvDate,
                                 Amount = o.Amount,
                                 DiscAmount = o.DiscAmount,
                                 PaymentOption = o.PaymentOption,
                                 BankID = o.BankID,
                                 BAccountID = o.BAccountID,
                                 ConfigID = o.ConfigID,
                                 ChequeNo = o.ChequeNo,
                                 ChType = o.ChType,
                                 CurID = o.CurID,
                                 CurRate = o.CurRate,
                                 TaxAuth = o.TaxAuth,
                                 TaxClass = o.TaxClass,
                                 TaxRate = o.TaxRate,
                                 TaxAccID = o.TaxAccID,
                                 TaxAmount = o.TaxAmount,
                                 DocDate = o.DocDate,
                                 PostDate = o.PostDate,
                                 Narration = o.Narration,
                                 RefNo = o.RefNo,
                                 PayReason = o.PayReason,
                                /// Posted = o.Posted,
                                 PostedBy = o.PostedBy,
                                 PostedDate = o.PostedDate,
                                 LinkDetID = o.LinkDetID,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 DocStatus = o.DocStatus,
                                 CprID = o.CprID,
                                 CprNo = o.CprNo,
                                 CprDate = o.CprDate,
                                 Id = o.Id
                             }
                         });

            var apinvhListDtos = await query.ToListAsync();

            return _apinvhExcelExporter.ExportToFile(apinvhListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_APINVH_Edit)]
        public APINVHDto GetDocNoAmount(int id)
        {

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new APINVHDto();

            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("RecVsRetAmount", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RECDOCNO", id);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {
                            result = new APINVHDto();
                            result.RecAmt = dataReader["RecAmount"] is DBNull ? "" : dataReader["RecAmount"].ToString();
                            result.RetAmt = dataReader["RetAmount"] is DBNull ? "" : dataReader["RetAmount"].ToString();
                            result.BalAmt = dataReader["BalAmount"] is DBNull ? "" : dataReader["BalAmount"].ToString();
                            result.VAccountID = dataReader["accountid"] is DBNull ? "" : dataReader["accountid"].ToString();
                            result.SubAccID = dataReader["subaccid"] is DBNull ? 0 :Convert.ToInt32(dataReader["subaccid"].ToString());
                            result.PendingAmt = dataReader["PendingAmt"] is DBNull ? 0 :Convert.ToInt32(dataReader["PendingAmt"].ToString());
                            result.CountRec = dataReader["CountRec"] is DBNull ? 0 :Convert.ToInt32(dataReader["CountRec"].ToString());
                            result.AccountName=dataReader["accountName"] is DBNull ?"" : dataReader["accountName"].ToString();
                            result.SubaccName=dataReader["subAccName"] is DBNull ?"" : dataReader["subAccName"].ToString();
                            result.PartyInvNo=dataReader["docNo"] is DBNull ?"" : dataReader["docNo"].ToString();
                            result.PartyInvDate = dataReader["DocDate"] is DBNull ?DateTime.Now :Convert.ToDateTime(dataReader["DocDate"].ToString());
                        }

                    }
                    //// cn.Close();
                }
            }
            return result;

        }
    }
}
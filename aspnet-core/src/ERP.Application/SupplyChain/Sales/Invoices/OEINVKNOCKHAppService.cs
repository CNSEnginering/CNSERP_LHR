using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.Invoices.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.GeneralLedger.SetupForms;
using ERP.Authorization.Users;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;

namespace ERP.SupplyChain.Sales.Invoices
{
    [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH)]
    public class OEINVKNOCKHAppService : ERPAppServiceBase, IOEINVKNOCKHAppService
    {
        private readonly IRepository<OEINVKNOCKH> _oeinvknockhRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<OEINVKNOCKD> _oeinvknockdRepository;
        private readonly IRepository<User, long> _userRepository;
        private VoucherEntryAppService _voucherEntryAppService;
        public OEINVKNOCKHAppService(IRepository<OEINVKNOCKH> oeinvknockhRepository, IRepository<User, long> userRepository, VoucherEntryAppService voucherEntryAppService, IRepository<OEINVKNOCKD> oeinvknockdRepository, IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<GLLocation> glLocationRepository, IRepository<ChartofControl, string> chartofControlRepository)
        {
            _oeinvknockhRepository = oeinvknockhRepository;
            _glLocationRepository = glLocationRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _oeinvknockdRepository = oeinvknockdRepository;
            _userRepository = userRepository;
            _voucherEntryAppService = voucherEntryAppService;
        }

        public async Task<PagedResultDto<GetOEINVKNOCKHForViewDto>> GetAll(GetAllOEINVKNOCKHInput input)
        {

            var filteredOEINVKNOCKH = _oeinvknockhRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DebtorCtrlAc.Contains(input.Filter) || e.PaymentOption.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.BAccountID.Contains(input.Filter) || e.InsNo.Contains(input.Filter) || e.CurID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.PostedBy.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinGLLOCIDFilter != null, e => e.GLLOCID >= input.MinGLLOCIDFilter)
                        .WhereIf(input.MaxGLLOCIDFilter != null, e => e.GLLOCID <= input.MaxGLLOCIDFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinPostDateFilter != null, e => e.PostDate >= input.MinPostDateFilter)
                        .WhereIf(input.MaxPostDateFilter != null, e => e.PostDate <= input.MaxPostDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DebtorCtrlAcFilter), e => e.DebtorCtrlAc == input.DebtorCtrlAcFilter)
                        .WhereIf(input.MinCustIDFilter != null, e => e.CustID >= input.MinCustIDFilter)
                        .WhereIf(input.MaxCustIDFilter != null, e => e.CustID <= input.MaxCustIDFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentOptionFilter), e => e.PaymentOption == input.PaymentOptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID == input.BankIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BAccountIDFilter), e => e.BAccountID == input.BAccountIDFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(input.MinInsTypeFilter != null, e => e.InsType >= input.MinInsTypeFilter)
                        .WhereIf(input.MaxInsTypeFilter != null, e => e.InsType <= input.MaxInsTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InsNoFilter), e => e.InsNo == input.InsNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurIDFilter), e => e.CurID == input.CurIDFilter)
                        .WhereIf(input.MinCurRateFilter != null, e => e.CurRate >= input.MinCurRateFilter)
                        .WhereIf(input.MaxCurRateFilter != null, e => e.CurRate <= input.MaxCurRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(input.PostedFilter.HasValue && input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostedByFilter), e => e.PostedBy == input.PostedByFilter)
                        .WhereIf(input.MinPostedDateFilter != null, e => e.PostedDate >= input.MinPostedDateFilter)
                        .WhereIf(input.MaxPostedDateFilter != null, e => e.PostedDate <= input.MaxPostedDateFilter)
                        .WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
                        .WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var pagedAndFilteredOEINVKNOCKH = filteredOEINVKNOCKH
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var oeinvknockh = from o in pagedAndFilteredOEINVKNOCKH
                              select new
                              {

                                  o.DocNo,
                                  o.GLLOCID,
                                  o.DocDate,
                                  o.PostDate,
                                  o.DebtorCtrlAc,
                                  o.CustID,
                                  o.Amount,
                                  o.PaymentOption,
                                  o.BankID,
                                  o.BAccountID,
                                  o.ConfigID,
                                  o.InsType,
                                  o.InsNo,
                                  o.CurID,
                                  o.CurRate,
                                  o.Narration,
                                  o.Posted,
                                  o.PostedBy,
                                  o.PostedDate,
                                  o.LinkDetID,
                                  o.CreatedBy,
                                  o.CreatedDate,
                                  o.AudtUser,
                                  o.AudtDate,
                                  Id = o.Id
                              };

            var totalCount = await filteredOEINVKNOCKH.CountAsync();

            var dbList = await oeinvknockh.ToListAsync();
            var results = new List<GetOEINVKNOCKHForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOEINVKNOCKHForViewDto()
                {
                    OEINVKNOCKH = new OEINVKNOCKHDto
                    {

                        DocNo = o.DocNo,
                        GLLOCID = o.GLLOCID,
                        DocDate = o.DocDate,
                        PostDate = o.PostDate,
                        DebtorCtrlAc = o.DebtorCtrlAc,
                        CustID = o.CustID,
                        Amount = o.Amount,
                        PaymentOption = o.PaymentOption,
                        BankID = o.BankID,
                        BAccountID = o.BAccountID,
                        ConfigID = o.ConfigID,
                        InsType = o.InsType,
                        InsNo = o.InsNo,
                        CurID = o.CurID,
                        CurRate = o.CurRate,
                        Narration = o.Narration,
                        Posted = o.Posted,
                        PostedBy = o.PostedBy,
                        PostedDate = o.PostedDate,
                        LinkDetID = o.LinkDetID,
                        CreatedBy = o.CreatedBy,
                        CreatedDate = o.CreatedDate,
                        AudtUser = o.AudtUser,
                        AudtDate = o.AudtDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOEINVKNOCKHForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOEINVKNOCKHForViewDto> GetOEINVKNOCKHForView(int id)
        {
            var oeinvknockh = await _oeinvknockhRepository.GetAsync(id);

            var output = new GetOEINVKNOCKHForViewDto { OEINVKNOCKH = ObjectMapper.Map<OEINVKNOCKHDto>(oeinvknockh) };

            return output;
        }

        //[AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH_Edit)]
        //public async Task<GetOEINVKNOCKHForEditOutput> GetOEINVKNOCKHForEdit(EntityDto input)
        //{
        //    var oeinvknockh = await _oeinvknockhRepository.FirstOrDefaultAsync(input.Id);

        //    var output = new GetOEINVKNOCKHForEditOutput { OEINVKNOCKH = ObjectMapper.Map<CreateOrEditOEINVKNOCKHDto>(oeinvknockh) };

        //    return output;
        //}

        [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH_Edit)]
        public async Task<CreateOrEditOEINVKNOCKHDto> GetOEINVKNOCKHForEdit(EntityDto input)
        {
            var oeinvknockh = await _oeinvknockhRepository.FirstOrDefaultAsync(input.Id);

            var output = ObjectMapper.Map<CreateOrEditOEINVKNOCKHDto>(oeinvknockh);

            //Location Description 
            output.LocDesc = _glLocationRepository.GetAll().Where(o => o.LocId == output.GLLOCID).Count() > 0
              ?
              _glLocationRepository.GetAll().Where(o => o.LocId == output.GLLOCID).FirstOrDefault().LocDesc
              : "";

            //Debtor Description
            output.DebtorDesc = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.SLType == 1 && o.Inactive == false && o.Id == output.DebtorCtrlAc).Count() > 0
                ? _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.SLType == 1 && o.Inactive == false && o.Id == output.DebtorCtrlAc).FirstOrDefault().AccountName
                : "";
            // Customer Description
            output.CustomerDesc = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Active == true && x.SLType == 1 && x.Id == output.CustID).Count() > 0
                ? _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Active == true && x.SLType == 1 && x.Id == output.CustID).FirstOrDefault().SubAccName
                : "";

            var detailInvoices = _oeinvknockdRepository.GetAll().Where(x => x.DetID == input.Id && x.TenantId == (int)AbpSession.TenantId);
            output.InvoiceKnockOffDetailDto = ObjectMapper.Map<List<OEINVKNOCKDDto>>(detailInvoices);
            return output;
        }
        public async Task CreateOrEdit(CreateOrEditOEINVKNOCKHDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH_Create)]
        //protected virtual async Task Create(CreateOrEditOEINVKNOCKHDto input)
        //{
        //    var oeinvknockh = ObjectMapper.Map<OEINVKNOCKH>(input);

        //    if (AbpSession.TenantId != null)
        //    {
        //        oeinvknockh.TenantId = (int?)AbpSession.TenantId;
        //    }

        //    await _oeinvknockhRepository.InsertAsync(oeinvknockh);

        //}

        [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH_Create)]
        protected virtual async Task Create(CreateOrEditOEINVKNOCKHDto input)
        {
            var oeinvknockh = ObjectMapper.Map<OEINVKNOCKH>(input);

            if (AbpSession.TenantId != null)
            {
                oeinvknockh.TenantId = (int)AbpSession.TenantId;
                oeinvknockh.CreatedDate = DateTime.Now;
                oeinvknockh.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            }
            var getId = await _oeinvknockhRepository.InsertAndGetIdAsync(oeinvknockh);

            if (input.InvoiceKnockOffDetailDto != null)
            {
                foreach (var data in input.InvoiceKnockOffDetailDto)
                {
                    var InvoiceDetail = ObjectMapper.Map<OEINVKNOCKD>(data);
                    if (AbpSession.TenantId != null)
                    {
                        InvoiceDetail.TenantId = (int)AbpSession.TenantId;
                        InvoiceDetail.CreatedDate = DateTime.Now;
                        InvoiceDetail.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    }
                    InvoiceDetail.DetID = getId;
                    InvoiceDetail.DocNo = input.DocNo;
                    await _oeinvknockdRepository.InsertAsync(InvoiceDetail);
                }
            }
        }

        //[AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH_Edit)]
        //protected virtual async Task Update(CreateOrEditOEINVKNOCKHDto input)
        //{
        //    var oeinvknockh = await _oeinvknockhRepository.FirstOrDefaultAsync((int)input.Id);
        //    ObjectMapper.Map(input, oeinvknockh);

        //}

        [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH_Edit)]
        protected virtual async Task Update(CreateOrEditOEINVKNOCKHDto input)
        {
            var oeinvknockh = await _oeinvknockhRepository.FirstOrDefaultAsync((int)input.Id);
            oeinvknockh.AudtDate = DateTime.Now;
            oeinvknockh.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

            ObjectMapper.Map(input, oeinvknockh);

            var InvoiceDetail = await _oeinvknockdRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DetID == input.Id).ToListAsync();
            if (InvoiceDetail != null)
            {
                foreach (var item in InvoiceDetail)
                {
                    await _oeinvknockdRepository.DeleteAsync(item);
                }
            }

            if (input.InvoiceKnockOffDetailDto != null)
            {
                foreach (var data in input.InvoiceKnockOffDetailDto)
                {
                    var InvoiceDetail1 = ObjectMapper.Map<OEINVKNOCKD>(data);
                    if (AbpSession.TenantId != null)
                    {
                        InvoiceDetail1.TenantId = (int)AbpSession.TenantId;
                        InvoiceDetail1.CreatedDate = DateTime.Now;
                        InvoiceDetail1.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    }
                    InvoiceDetail1.DetID = oeinvknockh.Id;
                    InvoiceDetail1.DocNo = input.DocNo;
                    await _oeinvknockdRepository.InsertAsync(InvoiceDetail1);
                }
            }

        }
        public List<OEINVKNOCKDDto> GetPostedInvoices(string Debtor, int CustId)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<OEINVKNOCKDDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("SP_InvoiceKnockOff", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Debtor", Debtor);
                    cmd.Parameters.AddWithValue("@CustID", CustId);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {

                            result.Add(new OEINVKNOCKDDto
                            {
                                InvNo = Convert.ToInt32(dataReader["InvNo"]),
                                Date = dataReader["InvDate"].ToString(),
                                Amount = Convert.ToDouble(dataReader["Amount"]),
                                AlreadyPaid = Convert.ToDouble(dataReader["AlreadyPaid"]),
                                Pending = Convert.ToDouble(dataReader["Pending"])
                            });
                        }
                    }
                }
            }
            return result;
        }
        //[AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //    await _oeinvknockhRepository.DeleteAsync(input.Id);
        //}

        [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKH_Delete)]
        public async Task Delete(EntityDto input)
        {
            var InvoiceDetail = await _oeinvknockdRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DetID == input.Id).ToListAsync();
            if (InvoiceDetail != null)
            {
                foreach (var item in InvoiceDetail)
                {
                    await _oeinvknockdRepository.DeleteAsync(item);
                }
            }
            await _oeinvknockhRepository.DeleteAsync(input.Id);
        }
        public int GetDocId()
        {
            var result = _oeinvknockhRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }
        public string ProcessInvoice(CreateOrEditOEINVKNOCKHDto input)
        {
            string alertMsg = "";
            alertMsg = ProcessInvoices(input);
            return alertMsg;
        }
        private string ProcessInvoices(CreateOrEditOEINVKNOCKHDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).Count() > 0 ?
                _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName : "";
            var glinvHedaer = _oeinvknockhRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id).Count() > 0 ?
                _oeinvknockhRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id).SingleOrDefault() : null;

            //update for tax
            if (glinvHedaer != null)
            {
                ObjectMapper.Map(input, glinvHedaer);
            }


            //string narration = glinvHedaer.Narration + "-" + glinvHedaer.PartyInvNo + "-" + glinvHedaer.RefNo + "-" + glinvHedaer.ChequeNo;
            string refrence = "";
            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            double? totalAmount = input.Amount;
            if (input.CustID != 0)
            {
                refrence = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
                && o.Id == input.CustID
                && o.AccountID == input.DebtorCtrlAc
                ).Count() > 0 ?
                    _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
                    && o.Id == input.CustID
                    && o.AccountID == input.DebtorCtrlAc).First().SubAccName :
                    "";

                if (totalAmount > 0)
                {
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = totalAmount > 0 ? (-totalAmount) : totalAmount,
                        AccountID = input.DebtorCtrlAc,
                        Narration = input.Narration,
                        SubAccID = input.CustID,
                        LocId = input.GLLOCID.Value,
                        IsAuto = false
                    });
                }
                if (!string.IsNullOrWhiteSpace(input.BAccountID))
                {
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = totalAmount > 0 ? totalAmount : 0,
                        AccountID = input.BAccountID,
                        Narration = input.Narration,
                        SubAccID = 0,
                        LocId = input.GLLOCID.Value,
                        IsAuto = false
                    });
                }
            }



            VoucherEntryDto autoEntry = new VoucherEntryDto()
            {
                GLTRHeader = new CreateOrEditGLTRHeaderDto
                {
                    BookID = glinvHedaer.PaymentOption.Trim() == "Bank" ? "BR" : "CR",
                    NARRATION = input.Narration,
                    DocDate = input.DocDate.Value,
                    DocMonth = input.DocDate.Value.Month,
                    Approved = true,
                    AprovedBy = user,
                    AprovedDate = DateTime.Now,
                    Posted = false,
                    PostedBy = user,
                    PostedDate = DateTime.Now,
                    LocId = glinvHedaer.GLLOCID.Value,
                    CreatedBy = user,
                    CreatedOn = DateTime.Now,
                    AuditUser = user,
                    AuditTime = DateTime.Now,
                    CURID = currency.Id,
                    CURRATE = currency.CurrRate,
                    ConfigID = 0,
                    Reference = refrence,
                    Amount = glinvHedaer.Amount == null ? 0 : Convert.ToDecimal(glinvHedaer.Amount),
                    FmtDocNo = _voucherEntryAppService.GetMaxDocId(glinvHedaer.PaymentOption.Trim() == "Bank" ? "BR" : "CR", true, input.DocDate)
                },
                GLTRDetail = gltrdetailsList
            };

            if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null)
            {
                var voucher = _voucherEntryAppService.ProcessVoucherEntry(autoEntry);

                glinvHedaer.Posted = true;
                glinvHedaer.PostedBy = user;
                glinvHedaer.PostedDate = DateTime.Now;
                glinvHedaer.LinkDetID = voucher[0].Id;
                var glinvh = _oeinvknockhRepository.FirstOrDefault((int)glinvHedaer.Id);
                ObjectMapper.Map(glinvHedaer, glinvh);

                alertMsg = "Save";
            }
            else
            {
                alertMsg = "NoRecord";
            }
            return alertMsg;
        }
    }
}
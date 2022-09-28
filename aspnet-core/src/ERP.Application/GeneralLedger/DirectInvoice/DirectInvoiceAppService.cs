using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using AutoMapper;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.CommonServices;
using ERP.GeneralLedger.DirectInvoice;
using ERP.GeneralLedger.DirectInvoice.Dtos;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.SupplyChain.Inventory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.GeneralLedger
{
    [AbpAuthorize(AppPermissions.Pages_DirectInvoice)]
    public class DirectInvoiceAppService : ERPAppServiceBase, IDirectInvoiceAppService
    {
        private readonly IRepository<GLINVHeader> _glinvHeaderRepository;
        private readonly IRepository<GLINVDetail> _glinvDetailRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<GLCONFIG> _glconfigRepository;
        private readonly IRepository<GLOption> _glOptionRepository;
        private readonly IRepository<ICSetup> _icsetupRepository;

        private VoucherEntryAppService _voucherEntryAppService;

        public DirectInvoiceAppService(IRepository<GLINVHeader> glinvHeaderRepository, IRepository<GLINVDetail> glinvDetailRepository, VoucherEntryAppService voucherEntryAppService, IRepository<User, long> userRepository, IRepository<GLCONFIG> glconfigRepository, IRepository<GLOption> glOptionRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<TaxAuthority, string> taxAuthorityRepository,
             IRepository<TaxClass> taxClassRepository,
             IRepository<ICSetup> icsetupRepository)
        {
            _glinvHeaderRepository = glinvHeaderRepository;
            _glinvDetailRepository = glinvDetailRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _userRepository = userRepository;
            _glconfigRepository = glconfigRepository;
            _voucherEntryAppService = voucherEntryAppService;
            _glOptionRepository = glOptionRepository;
            _taxAuthorityRepository = taxAuthorityRepository;
            _taxClassRepository = taxClassRepository;
            _icsetupRepository = icsetupRepository;
        }

        public async Task CreateOrEditDirectInvoice(DirectInvoiceDto input)
        {
            if (input.GLINVHeader.Id == null)
            {
                await CreateDirectInvoice(input);
            }
            else
            {
                await UpdateDirectInvoice(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DirectInvoice_Create)]
        private async Task CreateDirectInvoice(DirectInvoiceDto input)
        {
            var glinvHeader = ObjectMapper.Map<GLINVHeader>(input.GLINVHeader);
            glinvHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            glinvHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            glinvHeader.CreateDate = DateTime.Now;
            if (AbpSession.TenantId != null)
            {
                glinvHeader.TenantId = (int)AbpSession.TenantId;
            }

            glinvHeader.DocNo = GetMaxDocId(input.GLINVHeader.TypeID);
            var getGenratedId = await _glinvHeaderRepository.InsertAndGetIdAsync(glinvHeader);


            foreach (var item in input.GLINVDetail)
            {

                var gltrDetail = ObjectMapper.Map<GLINVDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    gltrDetail.TenantId = (int)AbpSession.TenantId;

                }
                gltrDetail.SubAccID = item.SubAccID != null ? item.SubAccID : 0;

                gltrDetail.DetID = getGenratedId;
                await _glinvDetailRepository.InsertAsync(gltrDetail);
            }
        }
        [AbpAuthorize(AppPermissions.Pages_DirectInvoice_UpdateCpr)]
        public async Task UpdateCpr(DirectInvoiceDto input)
        {
            var glinvHeader = await _glinvHeaderRepository.FirstOrDefaultAsync((int)input.GLINVHeader.Id);
            input.GLINVHeader.CreateDate = glinvHeader.CreateDate;
            ObjectMapper.Map(input.GLINVHeader, glinvHeader);

        }
        public List<string> GetUpdate(string accountId, int subAccId)
        {
            List<string> updates = new List<string>(6);
            string taxAuthDescp = "";
            string taxClassDescp = "";
            string taxRate = "";
            string taxAccID = "";
            string taxAuthID = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).Count() > 0 ?
                                       _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).SingleOrDefault().TAXAUTH : "";
            if (taxAuthID != "" && taxAuthID != null)
            {
                taxAuthDescp = _taxAuthorityRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == taxAuthID).SingleOrDefault().TAXAUTHDESC;
            }
            int? taxClassId = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).Count() > 0 ?
                                       _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).SingleOrDefault().ClassID : null;
            if (taxClassId != null)
            {
                taxClassDescp = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == taxClassId && o.TAXAUTH == taxAuthID).SingleOrDefault().CLASSDESC;
                taxRate = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == taxClassId && o.TAXAUTH == taxAuthID).SingleOrDefault().CLASSRATE.ToString();
                taxAccID = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == taxClassId && o.TAXAUTH == taxAuthID).SingleOrDefault().TAXACCID;
            }

            updates.Add(taxAuthID);
            updates.Add(taxAuthDescp);
            updates.Add(taxClassId.ToString());
            updates.Add(taxClassDescp);
            updates.Add(taxRate);
            updates.Add(taxAccID);

            return updates;
        }

        public List<string> GetSalesUpdate(string accountId, int subAccId)
        {
            List<string> salesUpdates = new List<string>(6);
            string salesTaxAuthDescp = "";
            string salesTaxClassDescp = "";
            string salesTaxRate = "";
            string salesTaxAccID = "";
            string salesTaxAuthID = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).Count() > 0 ?
                                       _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).SingleOrDefault().STTAXAUTH : "";
            if (salesTaxAuthID != "" && salesTaxAuthID != null)
            {
                salesTaxAuthDescp = _taxAuthorityRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == salesTaxAuthID).SingleOrDefault().TAXAUTHDESC;
            }
            int? salesTaxClassId = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).Count() > 0 ?
                                       _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).SingleOrDefault().STClassID : null;
            if (salesTaxClassId != null)
            {
                salesTaxClassDescp = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == salesTaxClassId && o.TAXAUTH == salesTaxAuthID).Count() > 0 ?
                                       _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == salesTaxClassId && o.TAXAUTH == salesTaxAuthID).SingleOrDefault().CLASSDESC : null;
                salesTaxRate = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == salesTaxClassId && o.TAXAUTH == salesTaxAuthID).Count() > 0 ?
                                       _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == salesTaxClassId && o.TAXAUTH == salesTaxAuthID).SingleOrDefault().CLASSRATE.ToString() : null;
                salesTaxAccID = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == salesTaxClassId && o.TAXAUTH == salesTaxAuthID).Count() > 0 ?
                                       _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == salesTaxClassId && o.TAXAUTH == salesTaxAuthID).SingleOrDefault().TAXACCID : null;
            }

            salesUpdates.Add(salesTaxAuthID);
            salesUpdates.Add(salesTaxAuthDescp);
            salesUpdates.Add(salesTaxClassId.ToString());
            salesUpdates.Add(salesTaxClassDescp);
            salesUpdates.Add(salesTaxRate);
            salesUpdates.Add(salesTaxAccID);

            return salesUpdates;
        }

        public bool? GetCreditLimitCheck(string accountId, int subAccId)
        {

            bool? creditLimit = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).Count() > 0 ?
                                       _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).SingleOrDefault().CrLimit : null;
            return creditLimit;
        }

        public decimal? GetCreditLimit(string accountId, int subAccId)
        {
            decimal? creditLimit = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).Count() > 0 ?
                                       _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountId && o.Id == subAccId).SingleOrDefault().CreditLimit : 0;

            return creditLimit;
        }

        [AbpAuthorize(AppPermissions.Pages_DirectInvoice_Edit)]
        private async Task UpdateDirectInvoice(DirectInvoiceDto input)
        {
            var glinvHeader = await _glinvHeaderRepository.FirstOrDefaultAsync((int)input.GLINVHeader.Id);
            ObjectMapper.Map(input.GLINVHeader, glinvHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.GLINVDetail.Select(o => o.Id).ToArray();
            var detailDBRecords = _glinvDetailRepository.GetAll().Where(o => o.DetID == input.GLINVHeader.Id && o.IsAuto == false).Where(o => !deltedRecordsArray.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _glinvDetailRepository.DeleteAsync(item.Id);
            }

            foreach (var item in input.GLINVDetail)
            {
                if (item.IsAuto == true)
                {
                    var autoEntryId = _glinvDetailRepository.GetAll().Where(o => o.DetID == input.GLINVHeader.Id && o.IsAuto == true).SingleOrDefault().Id;
                    var gltrDetail = await _glinvDetailRepository.FirstOrDefaultAsync(autoEntryId);
                    item.Id = autoEntryId;
                    item.DetID = (int)input.GLINVHeader.Id;
                    ObjectMapper.Map(item, gltrDetail);
                }
                else
                {
                    if (item.Id != null)
                    {
                        var gltrDetail = await _glinvDetailRepository.FirstOrDefaultAsync((int)item.Id);
                        ObjectMapper.Map(item, gltrDetail);
                    }
                    else
                    {
                        var gltrDetail = ObjectMapper.Map<GLINVDetail>(item);

                        if (AbpSession.TenantId != null)
                        {
                            gltrDetail.TenantId = (int)AbpSession.TenantId;

                        }

                        gltrDetail.SubAccID = item.SubAccID != null ? item.SubAccID : 0;

                        gltrDetail.DetID = (int)input.GLINVHeader.Id;
                        await _glinvDetailRepository.InsertAsync(gltrDetail);
                    }
                }
            }
        }

        public double? GetClosingBalance(string accountId, int subAccId, DateTime date)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var balance = 0.0;
            using (SqlConnection cn = new SqlConnection(str))
            {

                SqlCommand cmd = new SqlCommand("select  dbo.PartyClosingBal ('" + accountId + "','" + subAccId + "','" + date + "'," + AbpSession.TenantId + ") as Balance", cn);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        balance = Convert.ToDouble(rdr["Balance"]);
                    }
                }
            }
            return balance;
        }

        public int GetMaxDocId(string typeId)
        {
            int maxid = 0;
            if (typeId != "")
            {
                maxid = ((from tab1 in _glinvHeaderRepository.GetAll().Where(o => o.TypeID == typeId && o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            }
            return maxid;
        }

        [AbpAuthorize(AppPermissions.Pages_DirectInvoice_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _glinvHeaderRepository.DeleteAsync(input.Id);
            var glinvDetailsList = _glinvDetailRepository.GetAll().Where(e => e.DetID == input.Id);
            foreach (var item in glinvDetailsList)
            {
                await _glinvDetailRepository.DeleteAsync(item.Id);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DirectInvoice_PaymentProcess)]
        public string ProcessDirectInvoice(DirectInvoiceDto input)
        {
            string alertMsg = "";
            if (input.Target == "Stock")
            {
                alertMsg = ProcessStock(input);
            }
            else if (input.Target == "Payment")
            {
                alertMsg = ProcessPayment(input);
            }
            return alertMsg;
        }


        private string ProcessStock(DirectInvoiceDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).Count() > 0 ?
                _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName : "";
            var stockAccount = _glOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count() > 0 ?
                _glOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().STOCKCTRLACC : "";
            var glinvHedaer = _glinvHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == input.GLINVHeader.Id).Count() > 0 ?
                _glinvHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == input.GLINVHeader.Id).SingleOrDefault() : null;
            var glinvDetail = _glinvDetailRepository.GetAll().Where(o => o.DetID == input.GLINVHeader.Id);
            var configId = 0;
            string narration = glinvHedaer.Narration + "-" + glinvHedaer.PartyInvNo + "-" + glinvHedaer.RefNo + "-" + glinvHedaer.ChequeNo;

            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            //double? totalAmount = 0;
            //string accountID = "";
            //int? subAccountID = 0;
            //foreach (var item in glinvDetail)
            //{
            //    if (item.SubAccID != 0)
            //    {
            //        accountID = item.AccountID;
            //        subAccountID = item.SubAccID;
            //    }
            //    totalAmount += item.Amount;
            //}

            ////Credit Amount
            //gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            //{
            //    Amount = -totalAmount,
            //    AccountID = accountID,
            //    Narration = narration,
            //    SubAccID = subAccountID,
            //    LocId = 1,
            //    IsAuto = true
            //});

            ////Debit Amount
            //gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            //{
            //    Amount = totalAmount,
            //    AccountID = stockAccount,
            //    Narration = narration,
            //    SubAccID = 0,
            //    LocId = 1,
            //    IsAuto = false
            //});

            foreach (var item in glinvDetail)
            {
                if (item.Amount < 0)
                {
                    //Credit Amount
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = item.Amount,
                        AccountID = item.AccountID,
                        Narration = item.Narration,
                        SubAccID = item.SubAccID,
                        LocId = glinvHedaer.LocID.Value,
                        IsAuto = false
                    });
                }
                else if (item.Amount > 0)
                {
                    if (input.GLINVHeader.TypeID == "ST")
                    {
                        //Debit Amount
                        gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                        {
                            Amount = item.Amount + input.GLINVHeader.TaxAmount,
                            AccountID = item.AccountID,
                            Narration = item.Narration,
                            SubAccID = item.SubAccID,
                            LocId = glinvHedaer.LocID.Value,
                            IsAuto = false
                        });
                        glinvHedaer.InvAmount = item.Amount + input.GLINVHeader.TaxAmount;
                    }
                    else
                    {
                        //Debit Amount
                        gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                        {
                            Amount = item.Amount,
                            AccountID = item.AccountID,
                            Narration = item.Narration,
                            SubAccID = item.SubAccID,
                            LocId = glinvHedaer.LocID.Value,
                            IsAuto = false
                        });
                    }
                }
            }

            if (input.GLINVHeader.TypeID == "ST")
            {
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = -input.GLINVHeader.TaxAmount,
                    AccountID = input.GLINVHeader.TaxAccID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = glinvHedaer.LocID.Value,
                    IsAuto = false
                });

                ObjectMapper.Map(glinvHedaer, glinvHedaer);
            }


            VoucherEntryDto autoEntry = new VoucherEntryDto()
            {
                GLTRHeader = new CreateOrEditGLTRHeaderDto
                {
                    BookID = glinvHedaer.TypeID == "AP" ?
                    _icsetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).First().PRBookID
                    : _icsetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).First().SLBookID,
                    NARRATION = narration,
                    ChType = glinvHedaer.ChType,
                    DocDate = input.GLINVHeader.DocDate,
                    DocMonth = input.GLINVHeader.DocDate.Month,
                    Approved = true,
                    AprovedBy = user,
                    AprovedDate = DateTime.Now,
                    Posted = false,
                    PostedBy = user,
                    PostedDate = DateTime.Now,
                    LocId = glinvHedaer.LocID.Value,
                    CreatedBy = user,
                    CreatedOn = DateTime.Now,
                    AuditUser = user,
                    AuditTime = DateTime.Now,
                    CURID = currency.Id,
                    CURRATE = currency.CurrRate,
                    ConfigID = configId,
                    Reference = input.GLINVHeader.RefNo,
                    FmtDocNo = _voucherEntryAppService.GetMaxDocId(glinvHedaer.TypeID == "AP" ?
                    _icsetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).First().PRBookID
                    : _icsetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).First().SLBookID, true, input.GLINVHeader.DocDate)

                },
                GLTRDetail = gltrdetailsList
            };

            if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null)
            {
                var voucher = _voucherEntryAppService.ProcessVoucherEntry(autoEntry);

                glinvHedaer.PostedStock = true;
                glinvHedaer.PostedStockBy = user;
                glinvHedaer.PostedStockDate = DateTime.Now;
                glinvHedaer.LinkDetStkID = voucher[0].Id;
                var glinvh = _glinvHeaderRepository.FirstOrDefault((int)glinvHedaer.Id);
                ObjectMapper.Map(glinvHedaer, glinvh);

                alertMsg = "Save";
            }
            else
            {
                alertMsg = "NoRecord";
            }
            return alertMsg;
        }

        private string ProcessPayment(DirectInvoiceDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).Count() > 0 ?
                _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName : "";
            var glinvHedaer = _glinvHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == input.GLINVHeader.Id).Count() > 0 ?
                _glinvHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == input.GLINVHeader.Id).SingleOrDefault() : null;
            glinvHedaer.ChNumber = input.GLINVHeader.ChNumber;
            glinvHedaer.ChType = input.GLINVHeader.ChType;
            //update for tax
            if (glinvHedaer != null)
            {
                ObjectMapper.Map(input.GLINVHeader, glinvHedaer);
            }

            glinvHedaer = _glinvHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == input.GLINVHeader.Id).Count() > 0 ?
                _glinvHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == input.GLINVHeader.Id).SingleOrDefault() : null;

            var glinvDetail = _glinvDetailRepository.GetAll().Where(o => o.DetID == input.GLINVHeader.Id);
            var configId = _glconfigRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == glinvHedaer.AccountID && o.BookID == "BP").Count() > 0 ?
                _glconfigRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == glinvHedaer.AccountID && o.BookID == "BP").FirstOrDefault().ConfigID : 1;
            string narration = glinvHedaer.Narration + "-" + glinvHedaer.PartyInvNo + "-" + glinvHedaer.RefNo + "-" + glinvHedaer.ChequeNo;
            string refrence = "";
            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            double? totalAmount = 0;
            foreach (var item in glinvDetail)
            {
                //Party Amount
                if (item.SubAccID != 0)
                {
                    refrence = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
                    && o.Id == item.SubAccID
                    && o.AccountID == item.AccountID
                    ).Count() > 0 ?
                        _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
                        && o.Id == item.SubAccID
                        && o.AccountID == item.AccountID).First().SubAccName :
                        "";
                    if (glinvHedaer.TypeID == "AP")
                    {
                        if (item.Amount < 0)
                        {
                            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                            {
                                Amount = (-item.Amount),
                                AccountID = item.AccountID,
                                Narration = item.Narration,
                                SubAccID = item.SubAccID,
                                LocId = glinvHedaer.LocID.Value,
                                IsAuto = false
                            });
                        }
                    }
                    else if (glinvHedaer.TypeID == "ST")
                    {
                        if (item.Amount > 0)
                        {
                            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                            {
                                Amount = (-glinvHedaer.InvAmount),
                                AccountID = item.AccountID,
                                Narration = item.Narration,
                                SubAccID = item.SubAccID,
                                LocId = glinvHedaer.LocID.Value,
                                IsAuto = false
                            });
                        }
                    }
                    else if (glinvHedaer.TypeID == "AR")
                    {
                        if (item.Amount > 0)
                        {
                            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                            {
                                Amount = item.Amount > 0 ? (-item.Amount) : item.Amount,
                                AccountID = item.AccountID,
                                Narration = item.Narration,
                                SubAccID = item.SubAccID,
                                LocId = glinvHedaer.LocID.Value,
                                IsAuto = false
                            });
                        }
                    }
                }

                if (item.Amount > 0)
                {
                    totalAmount += item.Amount;
                }
            }

            if (glinvHedaer.TypeID == "AP")
            {
                //Credit Amount with Bank (Bank-Tax)
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = -(totalAmount - glinvHedaer.TaxAmount),
                    AccountID = glinvHedaer.AccountID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = glinvHedaer.LocID.Value,
                    IsAuto = true
                });

                //Credit Tax Amount Entry
                if (glinvHedaer.TaxAccID != null)
                {
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = -glinvHedaer.TaxAmount,
                        AccountID = glinvHedaer.TaxAccID,
                        Narration = "Tax-" + narration,
                        SubAccID = 0,
                        LocId = glinvHedaer.LocID.Value,
                        IsAuto = false
                    });
                }

            }
            else if (glinvHedaer.TypeID == "ST")
            {
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = (glinvHedaer.InvAmount - (glinvHedaer.ArAmount + glinvHedaer.ICTaxAmount)),
                    AccountID = glinvHedaer.AccountID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = glinvHedaer.LocID.Value,
                    IsAuto = true
                });

                if (glinvHedaer.TaxAccID != null)
                {
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = glinvHedaer.ICTaxAmount,
                        AccountID = glinvHedaer.ICTaxAccID,
                        Narration = narration,
                        SubAccID = 0,
                        LocId = glinvHedaer.LocID.Value,
                        IsAuto = false
                    });
                }

                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = glinvHedaer.ArAmount,
                    AccountID = glinvHedaer.ArAccID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = glinvHedaer.LocID.Value,
                    IsAuto = false
                });


            }
            else if (glinvHedaer.TypeID == "AR")
            {
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = glinvHedaer.TaxAmount,
                    AccountID = glinvHedaer.TaxAccID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = glinvHedaer.LocID.Value,
                    IsAuto = false
                });

                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = totalAmount - glinvHedaer.TaxAmount,
                    AccountID = glinvHedaer.AccountID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = glinvHedaer.LocID.Value,
                    IsAuto = true
                });
            }

            //foreach (var item in glinvDetail)
            //{
            //    if (item.Amount < 0)
            //    {
            //        //Credit Amount
            //        gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            //        {
            //            Amount = item.Amount,
            //            AccountID = item.AccountID,
            //            Narration = narration,
            //            SubAccID = item.SubAccID,
            //            LocId = 1,
            //            IsAuto = false
            //        });
            //    }
            //    else if (item.Amount > 0)
            //    {
            //        //Debit Amount
            //        gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            //        {
            //            Amount = item.Amount,
            //            AccountID = item.AccountID,
            //            Narration = narration,
            //            SubAccID = item.SubAccID,
            //            LocId = 1,
            //            IsAuto = false
            //        });
            //    }
            //}

            VoucherEntryDto autoEntry = new VoucherEntryDto()
            {
                GLTRHeader = new CreateOrEditGLTRHeaderDto
                {
                    BookID = glinvHedaer.TypeID == "AP" ? (glinvHedaer.PaymentOption.Trim() == "Bank" ? "BP" : "CP") : (glinvHedaer.PaymentOption.Trim() == "Bank" ? "BR" : "CR"),
                    NARRATION = narration,
                    DocDate = input.GLINVHeader.DocDate,
                    DocMonth = input.GLINVHeader.DocDate.Month,
                    Approved = true,
                    AprovedBy = user,
                    AprovedDate = DateTime.Now,
                    Posted = false,
                    PostedBy = user,
                    PostedDate = DateTime.Now,
                    ChNumber = glinvHedaer.ChNumber,
                    ChType = glinvHedaer.ChType,
                    LocId = glinvHedaer.LocID.Value,
                    CreatedBy = user,
                    CreatedOn = DateTime.Now,
                    AuditUser = user,
                    AuditTime = DateTime.Now,
                    CURID = currency.Id,
                    CURRATE = currency.CurrRate,
                    ConfigID = configId,
                    Reference = refrence,
                    FmtDocNo = _voucherEntryAppService.GetMaxDocId(glinvHedaer.TypeID == "AP" ? (glinvHedaer.PaymentOption.Trim() == "Bank" ? "BP" : "CP") :
                (glinvHedaer.PaymentOption.Trim() == "Bank" ? "BR" : "CR"), true, input.GLINVHeader.DocDate)
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
                var glinvh = _glinvHeaderRepository.FirstOrDefault((int)glinvHedaer.Id);
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

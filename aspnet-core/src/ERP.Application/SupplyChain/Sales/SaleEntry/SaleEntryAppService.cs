using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.SupplyChain.Sales.SaleEntry.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Sales.SaleEntry
{
    [AbpAuthorize(AppPermissions.Sales_SaleEntry)]
    public class SaleEntryAppService : ERPAppServiceBase
    {
        private readonly IRepository<OESALEHeader> _oesaleHeaderRepository;
        private readonly IRepository<OESALEDetail> _oesaleDetailRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<OECOLL> _oecollRepository;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly IRepository<ItemPricing> _itemPricingRepository;
        private readonly CommonAppService _commonappRepository;

        public SaleEntryAppService(
            IRepository<OESALEHeader> oesaleHeaderRepository,
            IRepository<OESALEDetail> oesaleDetailRepository,
            IRepository<User, long> userRepository,
            IRepository<ICItem> itemRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<ICSetup> icSetupRepository,
            IRepository<OECOLL> oecollRepository,
            VoucherEntryAppService voucherEntryAppService,
            CommonAppService commonappRepository,
            IRepository<ItemPricing> itemPricingRepository
            )
        {
            _oesaleHeaderRepository = oesaleHeaderRepository;
            _oesaleDetailRepository = oesaleDetailRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _icSetupRepository = icSetupRepository;
            _oecollRepository = oecollRepository;
            _voucherEntryAppService = voucherEntryAppService;
            _itemPricingRepository = itemPricingRepository;
            _commonappRepository = commonappRepository;
        }

        public async Task CreateOrEditSaleEntry(SaleEntryDto input)
        {
            if (input.OESALEHeader.Id == null)
            {
                await CreateSaleEntry(input);
            }
            else
            {
                await UpdateSaleEntry(input);
            }
        }

        [AbpAuthorize(AppPermissions.Sales_SaleEntry_Create)]
        private async Task CreateSaleEntry(SaleEntryDto input)
        {
            var oesaleHeader = ObjectMapper.Map<OESALEHeader>(input.OESALEHeader);
            oesaleHeader.CreateDate = DateTime.Now;
            oesaleHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (AbpSession.TenantId != null)
            {
                oesaleHeader.TenantId = (int)AbpSession.TenantId;
            }

            var salesACC = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == input.OESALEHeader.LocID && o.TypeID == input.OESALEHeader.TypeID).Count() > 0 ?
                _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == input.OESALEHeader.LocID && o.TypeID == input.OESALEHeader.TypeID).SingleOrDefault().ChAccountID : "";
            oesaleHeader.SalesCtrlAcc = salesACC;

            oesaleHeader.DocNo = GetMaxDocId();
            var docNoHeader = oesaleHeader.DocNo;
            var getGenratedId = await _oesaleHeaderRepository.InsertAndGetIdAsync(oesaleHeader);


            foreach (var item in input.OESALEDetail)
            {

                var oesaleDetail = ObjectMapper.Map<OESALEDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    oesaleDetail.TenantId = (int)AbpSession.TenantId;
                }
                oesaleDetail.LocID = input.OESALEHeader.LocID;
                oesaleDetail.DocNo = docNoHeader;
                oesaleDetail.DetID = getGenratedId;
                if (item.Qty > 0 && item.ItemID != "")
                {
                    await _oesaleDetailRepository.InsertAsync(oesaleDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Sales_SaleEntry_Edit)]
        private async Task UpdateSaleEntry(SaleEntryDto input)
        {
            var oesaleHeader = await _oesaleHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.OESALEHeader.DocNo && x.TenantId == AbpSession.TenantId);

            var salesACC = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == input.OESALEHeader.LocID && o.TypeID == input.OESALEHeader.TypeID).Count() > 0 ?
                _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == input.OESALEHeader.LocID && o.TypeID == input.OESALEHeader.TypeID).SingleOrDefault().ChAccountID : "";
            input.OESALEHeader.SalesCtrlAcc = salesACC;
            input.OESALEHeader.AudtDate = DateTime.Now;
            input.OESALEHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            ObjectMapper.Map(input.OESALEHeader, oesaleHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.OESALEDetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.OESALEDetail.Where(o => o.Qty <= 0 || o.ItemID == "").Select(o => o.Id).ToArray();
            var detailDBRecords = _oesaleDetailRepository.GetAll().Where(o => o.DocNo == input.OESALEHeader.DocNo && o.TenantId == AbpSession.TenantId).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _oesaleDetailRepository.DeleteAsync(item.Id);
            }

            //OESALEDetail update
            foreach (var item in input.OESALEDetail)
            {
                if (item.Id != null)
                {
                    var oesaleDetail = await _oesaleDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    oesaleDetail.LocID = input.OESALEHeader.LocID;
                    if (item.Qty > 0)
                    {
                        ObjectMapper.Map(item, oesaleDetail);
                    }
                }
                else
                {
                    var oesaleDetail = ObjectMapper.Map<OESALEDetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        oesaleDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    oesaleDetail.LocID = input.OESALEHeader.LocID;
                    oesaleDetail.DocNo = input.OESALEHeader.DocNo;
                    oesaleDetail.DetID = (int)input.OESALEHeader.Id;
                    if (item.Qty > 0 && item.ItemID != "")
                    {
                        await _oesaleDetailRepository.InsertAsync(oesaleDetail);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        public decimal GetItemPriceRate(string PriceList, string ItemID)
        {
            decimal rate = 0;
            if (PriceList != null && ItemID != null)
            {
                rate = _itemPricingRepository.GetAll().Where(o => o.TenantId == o.TenantId && o.PriceList.Trim().ToUpper() == PriceList.Trim().ToUpper() && o.ItemID.Trim().ToUpper() == ItemID.Trim().ToUpper()).Count() > 0 ?
                _itemPricingRepository.GetAll().Where(o => o.TenantId == o.TenantId && o.PriceList.Trim().ToUpper() == PriceList.Trim().ToUpper() && o.ItemID.Trim().ToUpper() == ItemID.Trim().ToUpper()).SingleOrDefault().NetPrice.Value : 0;
            }
            return rate;
        }

        //public decimal GetItemPriceRateSP(string PriceList, string ItemID)
        //{
        //    decimal rate = 0;
        //    if (PriceList != null && ItemID != null)
        //    {
        //        rate = _itemPricingRepository.GetAll().Where(o => o.TenantId == o.TenantId && o.PriceList.Trim().ToUpper() == PriceList.Trim().ToUpper() && o.ItemID.Trim().ToUpper() == ItemID.Trim().ToUpper()).Count() > 0 ?
        //        _itemPricingRepository.GetAll().Where(o => o.TenantId == o.TenantId && o.PriceList.Trim().ToUpper() == PriceList.Trim().ToUpper() && o.ItemID.Trim().ToUpper() == ItemID.Trim().ToUpper()).SingleOrDefault().NetPrice.Value : 0;
        //    }
        //    return rate;
        //}


        //SqlCommand cmd;
        //var tenantId = AbpSession.TenantId;
        //string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
        //List<SalarySheetListingDto> SalarySheetListingDtoList = new List<SalarySheetListingDto>();
        public DataTable GetItemPriceRateSP(string PriceList, string ItemID)

        {
            SqlConnection CN = null;
            SqlCommand SqlCom;
            string S;
            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter();

            try
            {

                // Connection Open
                CN = new SqlConnection();
                string str = ConfigurationManager.AppSettings["ConnectionString"];

                CN.ConnectionString = str;
                CN.Open();

                var tenantId = AbpSession.TenantId;



                S = "SP_ITEM_PRICE";

                SqlCom = new SqlCommand();

                //SqlCom.Parameters.AddWithValue("@TENANTID", tenantId);
                //SqlCom.Parameters.AddWithValue("@PRICElIST", PriceList);
                //SqlCom.Parameters.AddWithValue("@ITEMID", ItemID);

                //SqlCom.Parameters.Add(new SqlParameter("@TENANTID", tenantId));


                SqlCom.Parameters.Add(new SqlParameter("@TENANTID", tenantId));
                SqlCom.Parameters.Add(new SqlParameter("@PRICElIST", PriceList.Trim()));
                SqlCom.Parameters.Add(new SqlParameter("@ITEMID", ItemID.Trim()));


                try
                {
                    // Execute Query

                    SqlCom.CommandText = S;
                    SqlCom.CommandType = CommandType.StoredProcedure;
                    SqlCom.Connection = CN;
                    DA.SelectCommand = SqlCom;



                    DA.Fill(DT);

                    //if(DT.Rows.Count>0)
                    //{
                    //    .Sale =  DT.Rows[0]["SalePrice"];
                    //    .Sale = DT.Rows[0]["SalePrice"];
                    //    .Sale = DT.Rows[0]["SalePrice"];
                    //    .Sale = DT.Rows[0]["SalePrice"];
                    //    .Sale = DT.Rows[0]["SalePrice"];
                    //}




                }
                catch (Exception ex)
                {
                    // RetString = "ERROR: " & ex.Message.ToString
                    throw ex;
                }
                finally
                {
                    // Close Connection/Transactions
                    CN.Close();
                    DA = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Error
            // RetString = "ERROR: " & ex.Message.ToString
            finally
            {
                // Close/Remove Connection
                CN = null;
                DA = null;
            }

            return DT;
        }






        [AbpAuthorize(AppPermissions.Sales_SaleEntry_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _oesaleHeaderRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var oesaleDetailsList = _oesaleDetailRepository.GetAll().Where(e => e.DocNo == input.Id);
            foreach (var item in oesaleDetailsList)
            {
                await _oesaleDetailRepository.DeleteAsync(item.Id);
            }
        }
        public void DeleteLog(int detid)
        {
            var data = _oesaleHeaderRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "SaleEntry",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public string ProcessSaleEntry(CreateOrEditOESALEHeaderDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var transBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().SLBookID;
            var oesaleHeader = _oesaleHeaderRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var oesaleDetail = _oesaleDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var icItem = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var oecoll = _oecollRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.LocID == oesaleHeader.LocID && o.TypeID == oesaleHeader.TypeID);
            string narration = "Bill No. " + oesaleHeader.DocNo + " OGP No. " + oesaleHeader.OGP + " LocID " + oesaleHeader.LocID + " Total Qty: " + oesaleHeader.TotalQty + " Total Amount : " + oesaleHeader.TotAmt;

            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            //Credit Sale Amount
            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            {
                Amount = -oesaleDetail.Sum(o => o.Amount),
                AccountID = oecoll.SalesACC,
                Narration = narration,
                SubAccID = 0,
                LocId = oesaleHeader.LocID,
                IsAuto = false
            });

            //Credit Tax Amount
            if (oesaleDetail.Sum(o => o.TaxAmt) > 0)
            {
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = -oesaleDetail.Sum(o => o.TaxAmt),
                    AccountID = oecoll.PayableAcc,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = oesaleHeader.LocID,
                    IsAuto = false
                });
            }

            //Debit Party/Customer Amount
            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            {
                Amount = oesaleDetail.Sum(o => o.NetAmount),
                AccountID = oesaleHeader.SalesCtrlAcc,
                Narration = narration,
                SubAccID = oesaleHeader.CustID,
                LocId = Convert.ToInt32(oesaleHeader.LocID),
                IsAuto = false
            });


            //Debit Discount Amount
            if (input.Disc > 0)
            {
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = oesaleDetail.Sum(o => o.Disc),
                    AccountID = oecoll.DiscAcc,
                    Narration = narration,
                    LocId = Convert.ToInt32(oesaleHeader.LocID),
                    IsAuto = false
                });
            }

            //var transferDetailList = from o in oesaleDetail
            //                         join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
            //                         group o by new { i.Seg1Id } into gd
            //                         select new OESALEDetailDto
            //                         {
            //                             Amount = gd.Sum(x => x.Amount),
            //                             TaxAmt = gd.Sum(x => x.TaxAmt),
            //                             NetAmount = gd.Sum(x => x.NetAmount),
            //                             ItemID = gd.Key.Seg1Id,
            //                         };

            //foreach (var item in transferDetailList)
            //{
            //    //Credit Sale Amount
            //    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            //    {
            //        Amount = -Convert.ToDouble(item.Amount),
            //        AccountID = oecoll.SalesACC,
            //        Narration = narration,
            //        SubAccID = 0,
            //        LocId = oesaleHeader.LocID,
            //        IsAuto = false
            //    });

            //    //Credit Tax Amount
            //    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            //    {
            //        Amount = -Convert.ToDouble(item.TaxAmt),
            //        AccountID = oecoll.PayableAcc,
            //        Narration = narration,
            //        SubAccID = 0,
            //        LocId = oesaleHeader.LocID,
            //        IsAuto = false
            //    });

            //    //Debit Party/Customer Amount
            //    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            //    {
            //        Amount = Convert.ToDouble(item.NetAmount),
            //        AccountID = oesaleHeader.SalesCtrlAcc,
            //        Narration = narration,
            //        SubAccID = 0,
            //        LocId = Convert.ToInt32(oesaleHeader.LocID),
            //        IsAuto = false
            //    });
            //}

            VoucherEntryDto autoEntry = new VoucherEntryDto()
            {
                GLTRHeader = new CreateOrEditGLTRHeaderDto
                {
                    BookID = transBook,
                    NARRATION = narration,
                    DocDate = Convert.ToDateTime(input.DocDate),
                    DocMonth = Convert.ToDateTime(input.DocDate).Month,
                    Approved = true,
                    AprovedBy = user,
                    AprovedDate = DateTime.Now,
                    Posted = false,
                    LocId = oesaleHeader.LocID,
                    CreatedBy = user,
                    CreatedOn = DateTime.Now,
                    AuditUser = user,
                    AuditTime = DateTime.Now,
                    CURID = currency.Id,
                    CURRATE = currency.CurrRate,
                    ConfigID = 0
                },
                GLTRDetail = gltrdetailsList
            };

            if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null)
            {
                var voucher = _voucherEntryAppService.ProcessVoucherEntry(autoEntry);
                oesaleHeader.Posted = true;
                oesaleHeader.PostedBy = user;
                oesaleHeader.PostedDate = DateTime.Now;
                oesaleHeader.LinkDetID = voucher[0].Id;
                var transh = _oesaleHeaderRepository.FirstOrDefault((int)oesaleHeader.Id);
                ObjectMapper.Map(oesaleHeader, transh);

                alertMsg = "Save";
            }
            else
            {
                alertMsg = "NoRecord";
            }
            return alertMsg;
        }


        //public void ApprovalData(int[] postedData, string Mode, bool bit)
        //{
        //    try
        //    {
        //        var postedDataIds = postedData.Distinct();
        //        if (Mode == "UnApproval")
        //        {
        //            (from a in _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
        //             select a).ToList().ForEach(x =>
        //             {
        //                 x.Approved = false;
        //                 x.ApprovedDate = DateTime.Now;
        //                 x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
        //             });
        //        }
        //        else
        //        {
        //            (from a in _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
        //             select a).ToList().ForEach(x =>
        //             {
        //                 x.Approved = true;
        //                 x.ApprovedDate = DateTime.Now;
        //                 x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
        //             });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void ApprovalData(int[] postedData, string Mode, bool bit)
        {
            try
            {
                var postedDataIds = postedData.Distinct();
                // foreach (var data in postedDataIds)
                //  {
                //   var result = _icOPNHeaderRepository.GetAll().Where(o => o.Id == data).ToList();
                // var gltrHeader = await _icOPNHeaderRepository.FirstOrDefaultAsync((int)data);

                // foreach (var res in result)
                // {
                var DocNo = 0;
                if (Mode == "Posting")
                {
                    //res.Posted = bit;
                    //res.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.PostedDate = DateTime.Now;
                    //_repository.Update(res);
                }
                else if (Mode == "UnApproval")
                {
                    (from a in _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = false;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });
                    //res.Approved = false;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                else
                {
                    (from a in _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });
                    //res.Approved = bit;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                LogModel Log = new LogModel()
                {
                    Status = bit,
                    ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                    Detid = Convert.ToInt32(postedDataIds.FirstOrDefault().ToString()),
                    DocNo = DocNo,
                    FormName = "SaleEntry",
                    Action = Mode,
                    TenantId = AbpSession.TenantId
                };
                _commonappRepository.ApproveLog(Log);
                //  await _repository.SaveChangesAsync();
                //  }
                // }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}

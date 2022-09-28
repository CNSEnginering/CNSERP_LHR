using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Costing;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.SupplyChain.Sales.SaleReturn.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Sales.SaleReturn
{
    [AbpAuthorize(AppPermissions.Sales_SaleReturn)]
    public class SaleReturnAppService : ERPAppServiceBase
    {
        private readonly IRepository<OERETHeader> _oeretHeaderRepository;
        private readonly IRepository<OERETDetail> _oeretDetailRepository;
        private readonly CostingAppService _costingAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<OECOLL> _oecollRepository;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly CommonAppService _commonappRepository;

        public SaleReturnAppService(
            IRepository<OERETHeader> oeretHeaderRepository, 
            IRepository<OERETDetail> oeretDetailRepository, 
            CostingAppService costingAppService,
            IRepository<User, long> userRepository,
            IRepository<ICItem> itemRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<ICSetup> icSetupRepository,
            IRepository<OECOLL> oecollRepository,
            CommonAppService commonappRepository,
            VoucherEntryAppService voucherEntryAppService
            )
        {
            _oeretHeaderRepository = oeretHeaderRepository;
            _oeretDetailRepository = oeretDetailRepository;
            _costingAppService = costingAppService;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _icSetupRepository = icSetupRepository;
            _oecollRepository = oecollRepository;
            _voucherEntryAppService = voucherEntryAppService;
            _commonappRepository = commonappRepository;
        }

        public async Task CreateOrEditSaleReturn(SaleReturnDto input)
        {
            if (input.OERETHeader.Id == null)
            {
                await CreateSaleReturn(input);
            }
            else
            {
                await UpdateSaleReturn(input);
            }
        }

        [AbpAuthorize(AppPermissions.Sales_SaleReturn_Create)]
        private async Task CreateSaleReturn(SaleReturnDto input)
        {
            var oeretHeader = ObjectMapper.Map<OERETHeader>(input.OERETHeader);

            if (AbpSession.TenantId != null)
            {
                oeretHeader.TenantId = (int)AbpSession.TenantId;
            }

            var salesRetACC = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == input.OERETHeader.LocID && o.TypeID == input.OERETHeader.TypeID).Count() > 0 ?
                _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == input.OERETHeader.LocID && o.TypeID == input.OERETHeader.TypeID).SingleOrDefault().ChAccountID : "";
            oeretHeader.SalesCtrlAcc = salesRetACC;

            oeretHeader.DocNo = GetMaxDocId();
            oeretHeader.CreateDate = DateTime.Now;
            oeretHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var docNoHeader = oeretHeader.DocNo;
            var getGenratedId = await _oeretHeaderRepository.InsertAndGetIdAsync(oeretHeader);


            foreach (var item in input.OERETDetail)
            {

                var oeretDetail = ObjectMapper.Map<OERETDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    oeretDetail.TenantId = (int)AbpSession.TenantId;
                }

                

                oeretDetail.LocID = input.OERETHeader.LocID;
                oeretDetail.DocNo = docNoHeader;
                oeretDetail.DetID = getGenratedId;

                //calculate costing
                oeretDetail.Cost = _costingAppService.getCosting(Convert.ToDateTime(input.OERETHeader.DocDate), item.ItemID, oeretDetail.LocID,6,oeretDetail.DocNo);
                oeretDetail.CostAmt = oeretDetail.Cost * item.Qty;

                if (item.Qty > 0 && item.ItemID != "")
                {
                    await _oeretDetailRepository.InsertAsync(oeretDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Sales_SaleReturn_Edit)]
        private async Task UpdateSaleReturn(SaleReturnDto input)
        {
            var oeretHeader = await _oeretHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.OERETHeader.DocNo && x.TenantId == AbpSession.TenantId);

            var salesRetACC = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == oeretHeader.LocID && o.TypeID == oeretHeader.TypeID).Count() > 0 ?
                _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == oeretHeader.LocID && o.TypeID == oeretHeader.TypeID).SingleOrDefault().ChAccountID : "";
            input.OERETHeader.AudtDate = DateTime.Now;
            input.OERETHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.OERETHeader.SalesCtrlAcc = salesRetACC;

            ObjectMapper.Map(input.OERETHeader, oeretHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.OERETDetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.OERETDetail.Where(o => o.Qty <= 0 || o.ItemID == "").Select(o => o.Id).ToArray();
            var detailDBRecords = _oeretDetailRepository.GetAll().Where(o => o.DocNo == input.OERETHeader.DocNo && o.TenantId == AbpSession.TenantId).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _oeretDetailRepository.DeleteAsync(item.Id);
            }

            //OERETDetail update
            foreach (var item in input.OERETDetail)
            {
                if (item.Id != null)
                {
                    var oeretDetail = await _oeretDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    oeretDetail.LocID = input.OERETHeader.LocID;
                    if (item.Qty > 0)
                    {
                        ObjectMapper.Map(item, oeretDetail);
                    }
                }
                else
                {
                    var oeretDetail = ObjectMapper.Map<OERETDetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        oeretDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    oeretDetail.LocID = input.OERETHeader.LocID;
                    oeretDetail.DocNo = input.OERETHeader.DocNo;
                    oeretDetail.DetID = (int)input.OERETHeader.Id;

                    //calculate costing
                    oeretDetail.Cost = _costingAppService.getCosting(Convert.ToDateTime(input.OERETHeader.DocDate), item.ItemID, oeretDetail.LocID, 6, oeretDetail.DocNo);
                    oeretDetail.CostAmt = oeretDetail.Cost * item.Qty;

                    if (item.Qty > 0 && item.ItemID != "")
                    {
                        await _oeretDetailRepository.InsertAsync(oeretDetail);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _oeretHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        [AbpAuthorize(AppPermissions.Sales_SaleReturn_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _oeretHeaderRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var oeretDetailsList = _oeretDetailRepository.GetAll().Where(e => e.DocNo == input.Id);
            foreach (var item in oeretDetailsList)
            {
                await _oeretDetailRepository.DeleteAsync(item.Id);
            }
        }
        public void DeleteLog(int detid)
        {
            var data = _oeretHeaderRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "SaleReturn",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }
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
                    (from a in _oeretHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.Id))
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
                    (from a in _oeretHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.Id))
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
                    FormName = "SaleReturn",
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

        public string ProcessSaleReturn(CreateOrEditOERETHeaderDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var transBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().SRBookID;
            var oeretHeader = _oeretHeaderRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var oeretDetail = _oeretDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var icItem = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var oecoll = _oecollRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.LocID == oeretHeader.LocID && o.TypeID == oeretHeader.TypeID);
            string narration = "Bill No. " + oeretHeader.DocNo + " OGP No. " + oeretHeader.OGP + " LocID " + oeretHeader.LocID + " Total Qty: " + oeretHeader.TotalQty + " Total Amount : " + oeretHeader.TotAmt;

            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            //Debit Sale Amount
            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            {
                Amount = oeretDetail.Sum(o => o.Amount),
                AccountID = oecoll.SalesACC,
                Narration = narration,
                SubAccID = 0,
                LocId = oeretHeader.LocID,
                IsAuto = false
            });

            //Debit Tax Amount
            if(oeretDetail.Sum(o => o.TaxAmt) > 0)
            {
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = oeretDetail.Sum(o => o.TaxAmt),
                    AccountID = oecoll.PayableAcc,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = oeretHeader.LocID,
                    IsAuto = false
                });
            }

            //Credit Party/Customer Amount
            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            {
                Amount = -oeretDetail.Sum(o => o.NetAmount),
                AccountID = oeretHeader.SalesCtrlAcc,
                Narration = narration,
                SubAccID = oeretHeader.CustID,
                LocId = Convert.ToInt32(oeretHeader.LocID),
                IsAuto = false
            });

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
                    LocId = oeretHeader.LocID,
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
                oeretHeader.Posted = true;
                oeretHeader.PostedBy = user;
                oeretHeader.PostedDate = DateTime.Now;
                oeretHeader.LinkDetID = voucher[0].Id;
                var transh = _oeretHeaderRepository.FirstOrDefault((int)oeretHeader.Id);
                ObjectMapper.Map(oeretHeader, transh);

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

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.AccountPayables;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.CommonServices;
using ERP.Costing;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase.PurchaseOrder.Dtos;
using ERP.SupplyChain.Purchase.Requisition;
using ERP.SupplyChain.Purchase.Requisition.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Purchase.PurchaseOrder
{
    [AbpAuthorize(AppPermissions.Purchase_PurchaseOrders)]
    public class PurchaseOrderAppService : ERPAppServiceBase
    {
        private readonly IRepository<POPOHeader> _popoHeaderRepository;
        private readonly IRepository<POPODetail> _popoDetailRepository;
        private readonly IRepository<Requisitions> _requisitionRepository;
        private readonly IRepository<ReqStat> _reqStatRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<APTerm> _apTermRepository;
        private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;
        private readonly CostingAppService _costingService;
        private readonly IRepository<User, long> _userRepository;
        private readonly CommonAppService _commonappRepository;

        public PurchaseOrderAppService(IRepository<POPOHeader> popoHeaderRepository, IRepository<POPODetail> popoDetailRepository, CostingAppService costingService, IRepository<Requisitions> requisitionRepository, IRepository<ICItem> itemRepository, 
            IRepository<ReqStat> reqStatRepository, IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<APTerm> apTermRepository,
            IRepository<TaxAuthority, string> taxAuthorityRepository, IRepository<TaxClass> taxClassRepositor,CommonAppService commonappRepository,
             IRepository<User, long> userRepository)
        {
            _popoHeaderRepository = popoHeaderRepository;
            _popoDetailRepository = popoDetailRepository;
            _requisitionRepository = requisitionRepository;
            _itemRepository = itemRepository;
            _reqStatRepository = reqStatRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _apTermRepository = apTermRepository;
            _taxAuthorityRepository = taxAuthorityRepository;
            _taxClassRepository = taxClassRepositor;
            _costingService = costingService;
            _userRepository = userRepository;
            _commonappRepository = commonappRepository;
        }

        public async Task CreateOrEditPurchaseOrder(PurchaseOrderDto input)
        {
            if (input.POPOHeader.Id == null)
            {
                await CreatePurchaseOrder(input);
            }
            else
            {
                await UpdatePurchaseOrder(input);
            }
        }

        [AbpAuthorize(AppPermissions.Purchase_PurchaseOrders_Create)]
        private async Task CreatePurchaseOrder(PurchaseOrderDto input)
        {
            var popoHeader = ObjectMapper.Map<POPOHeader>(input.POPOHeader);

            if (AbpSession.TenantId != null)
            {
                popoHeader.TenantId = (int)AbpSession.TenantId;
            }

            popoHeader.DocNo = GetMaxDocId();
            var docHeader = popoHeader.DocNo;
            popoHeader.CreateDate = DateTime.Now;
            popoHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var getGenratedId = await _popoHeaderRepository.InsertAndGetIdAsync(popoHeader);

            if (getGenratedId > 0 && popoHeader.ReqNo > 0)
            {
                var requisition = _requisitionRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DocNo == popoHeader.ReqNo.Value).FirstOrDefault();
                requisition.Hold = true;
                _requisitionRepository.Update(requisition);
            }

            foreach (var item in input.POPODetail)
            {

                var popoDetail = ObjectMapper.Map<POPODetail>(item);

                if (AbpSession.TenantId != null)
                {
                    popoDetail.TenantId = (int)AbpSession.TenantId;
                }
                popoDetail.LocID = input.POPOHeader.LocID;
                popoDetail.DocNo = docHeader;
                popoDetail.DetID = getGenratedId;
                if (item.Qty > 0 && item.ItemID != "")
                {
                    await _popoDetailRepository.InsertAsync(popoDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Purchase_PurchaseOrders_Edit)]
        private async Task UpdatePurchaseOrder(PurchaseOrderDto input)
        {
            var popoHeader = await _popoHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.POPOHeader.DocNo && x.TenantId == AbpSession.TenantId);
            input.POPOHeader.AudtDate = DateTime.Now;
            input.POPOHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            ObjectMapper.Map(input.POPOHeader, popoHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.POPODetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.POPODetail.Where(o => o.Qty <= 0 || o.ItemID == "").Select(o => o.Id).ToArray();
            var detailDBRecords = _popoDetailRepository.GetAll().Where(o => o.DocNo == input.POPOHeader.DocNo && o.TenantId == AbpSession.TenantId).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _popoDetailRepository.DeleteAsync(item.Id);
            }

            foreach (var item in input.POPODetail)
            {
                if (item.Id != null)
                {
                    var popoDetail = await _popoDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    popoDetail.LocID = input.POPOHeader.LocID;
                    if (item.Qty > 0)
                    {
                        ObjectMapper.Map(item, popoDetail);
                    }
                }
                else
                {
                    var popoDetail = ObjectMapper.Map<POPODetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        popoDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    popoDetail.LocID = input.POPOHeader.LocID;
                    popoDetail.DocNo = input.POPOHeader.DocNo;
                    popoDetail.DetID = (int)input.POPOHeader.Id;
                    if (item.Qty > 0 && item.ItemID != "")
                    {
                        await _popoDetailRepository.InsertAsync(popoDetail);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _popoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        [AbpAuthorize(AppPermissions.Purchase_PurchaseOrders_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _popoHeaderRepository.DeleteAsync(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId);
            var popoDetailsList = _popoDetailRepository.GetAll().Where(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            //foreach (var item in popoDetailsList)
            //{
            //    await _popoDetailRepository.DeleteAsync(item.id);
            //}
        }
        public void DeleteLog(int detid)
        {
            var data = _popoHeaderRepository.FirstOrDefault(c => c.Id == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "PurchaseOrder",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        [AbpAuthorize(AppPermissions.Purchase_POPOHeaders_Edit)]
        public async Task<POPOHeaderDto> PendingReqEntries(int reqNo)
        {
            var reqHeader = _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == reqNo).Count()>0?
                _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == reqNo).SingleOrDefault():null;

            POPOHeaderDto output = null;
            if (reqHeader != null)
            {
                var reqStat = _reqStatRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

                var pendingReqEntries = from reqH in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == reqNo)
                                        join reqS in reqStat
                                        on new { X = reqH.LocID, Y = reqH.DocNo } equals new { X = reqS.LocID, Y = reqS.DocNo }
                                        where reqH.LocID == reqHeader.LocID && reqH.DocNo == reqHeader.DocNo && (reqS.Qty - reqS.POQty) > 0
                                        select new POPOHeaderDto
                                        {
                                            LocID = reqH.LocID,
                                            DocNo = reqH.DocNo,
                                            DocDate = reqH.DocDate,
                                            ArrivalDate = reqH.ExpArrivalDate,
                                            ReqNo = reqH.DocNo,
                                            OrdNo=reqH.OrdNo,
                                           // AccountID = popoHeader.AccountID,
                                           // AccDesc = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == popoHeader.AccountID).FirstOrDefault().AccountName,
                                           // SubAccID = popoHeader.SubAccID,
                                           // SubAccDesc = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == popoHeader.SubAccID && o.AccountID == popoHeader.AccountID).FirstOrDefault().SubAccName,
                                            TotalQty = reqH.TotalQty,
                                            // TotalAmt = popoHeader.TotalAmt,
                                            //OrdNo = reqH.OrdNo,
                                            CCID = reqH.CCID,
                                            Narration = reqH.Narration,
                                           // WHTermID = popoHeader.WHTermID,
                                           // WHTermDesc = _apTermRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == popoHeader.WHTermID).FirstOrDefault().TERMDESC,
                                           // WHRate = popoHeader.WHRate,
                                            //TaxAuth = popoHeader.TaxAuth,
                                            //TaxAuthDesc = _taxAuthorityRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == popoHeader.TaxAuth).FirstOrDefault().TAXAUTHDESC,
                                            //TaxClass = popoHeader.TaxClass,
                                            //TaxClassDesc = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == popoHeader.TaxClass).FirstOrDefault().CLASSDESC,
                                            //TaxRate = popoHeader.TaxRate,
                                            TaxAmount = 0,
                                            onHold = reqH.Hold,
                                          //  Active = reqH.Active,
                                            Completed = reqH.Completed,
                                            //AudtUser = reqH.AudtUser,
                                            //AudtDate = reqH.AudtDate,
                                            //CreatedBy = reqH.CreatedBy,
                                            //CreateDate = reqH.CreateDate,
                                            //Id = reqH.Id
                                        };
                var totalCount = await pendingReqEntries.CountAsync();

                output = ObjectMapper.Map<POPOHeaderDto>(await pendingReqEntries.FirstOrDefaultAsync());
            }
            return output;
        }

        public async Task<PagedResultDto<POPODetailDto>> PendingReqQty(int reqNo)
        {
            var requisition = _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == reqNo).Count()>0?
                _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == reqNo).SingleOrDefault():null;

            var pendingReqQty = from reqS in _reqStatRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                join icItm in _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                on reqS.ItemID equals icItm.ItemId
                                where reqS.LocID == requisition.LocID && reqS.DocNo == requisition.DocNo && (reqS.Qty - reqS.POQty) > 0
                                select new POPODetailDto
                                {
                                    LocID = reqS.LocID,
                                    DocNo = reqS.DocNo,
                                    ItemID = reqS.ItemID,
                                    ItemDesc=icItm.Descp,
                                    Unit = reqS.Unit,
                                    Conver = reqS.Conver,
                                    //Qty = reqS.Qty,
                                    Qty = reqS.Qty - reqS.POQty,
                                    //QtyP = reqS.Qty-reqS.POQty,
                                    Remarks = reqS.Remarks,
                                    //QIH = reqS.QIH,
                                    //POQty = reqS.POQty,
                                    Rate = 0,
                                    Amount = 0,
                                    TaxRate = 0,
                                    TaxAmt = 0,
                                    NetAmount = 0,
                                   // Id = reqS.Id
                                };
            var totalCount = await pendingReqQty.CountAsync();

            return new PagedResultDto<POPODetailDto>(
                totalCount,
                await pendingReqQty.ToListAsync()
            );
        }

        public PagedResultDto<CreateOrEditPOPOHeaderDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllRequisitionsInput input)
        {
            IQueryable<POPOHeader> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _popoHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _popoHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new CreateOrEditPOPOHeaderDto()
                                {
                                    //LocID = o.LocID,
                                    DocNo = o.DocNo,
                                    DocDate = o.DocDate,
                                    AudtUser = o.AudtUser,
                                    Narration = o.Narration,
                                    Id = o.Id
                                };

            var count = data.Count();
            // return ICOPNHeaderDtoList;
            return new PagedResultDto<CreateOrEditPOPOHeaderDto>(
              count,
              paginatedData.ToList()
          );
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
                    (from a in _popoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    (from a in _popoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    FormName = "PurchaseOrder",
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

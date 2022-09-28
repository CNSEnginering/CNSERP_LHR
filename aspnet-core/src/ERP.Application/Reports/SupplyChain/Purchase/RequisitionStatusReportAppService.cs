using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.SupplyChain.Purchase;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase;
using ERP.SupplyChain.Purchase.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class RequisitionStatusReportAppService : ERPReportAppServiceBase , IRequisitionStatusReportAppService
    {
        private IRepository<VwReqStatus> _vwReqStatusRepository;
        private IRepository<VwRetQty> _vwRetQtyRepository;
        private IRepository<ICItem> _itemRepository;
        private IRepository<ICLocation> _locRepository;
        private readonly IRepository<VwReqStatus2> _vwReqStatus2Repository;
        public RequisitionStatusReportAppService(IRepository<VwReqStatus> vwReqStatusRepository,
          IRepository<RequisitionDetail> requisitionDetailRepository,
          IRepository<ICItem> itemRepository,
          IRepository<ICLocation> locRepository,
          IRepository<VwRetQty> vwRetQtyRepository,
          IRepository<VwReqStatus2> vwReqStatus2Repository)
        {
            _vwReqStatusRepository = vwReqStatusRepository;
            _itemRepository = itemRepository;
            _locRepository = locRepository;
            _vwRetQtyRepository = vwRetQtyRepository;
            _vwReqStatus2Repository = vwReqStatus2Repository;
        }
        public List<RequisitionStatusReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _vwReqStatusRepository.GetAll().Where(a => a.TenantId == tenantId)
                       join
                       b in _locRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = Convert.ToInt32(a.LocID), B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                       join
                       c in _itemRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.ItemID, B = a.TenantId } equals new { A = c.ItemId, B = c.TenantId }
                       join 
                       d in _vwReqStatus2Repository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.ItemID, B = a.TenantId, C = a.LocID } equals new { A = d.ItemID, B = d.TenantId, C = Convert.ToInt32(d.Locid) }
                       join
                       e in _vwRetQtyRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.ItemID, B = a.TenantId, C = a.LocID } equals new { A = e.ItemID, B = e.TenantId, C = e.LocID }
                       into retQty
                       from sub in retQty.DefaultIfEmpty()
                       where (a.DocNo >= fromDoc && a.DocNo <= toDoc && a.TenantId == tenantId)
                       select new RequisitionStatusReport()
                       {
                           ItemId = a.ItemID,
                           Descp = c.Descp,
                           ReqDocNo = a.DocNo,
                           PODocNo = a.podocno,
                           ReqDocDate = a.DocDate.Value.Date.ToString(),
                           PODocDate = a.podate.Value.Date.ToString(),
                           RecDocDate = a.recdate.Value.Date.ToString(),
                           //Id = a.Id,
                           LocName = b.LocName,
                           OrdNo = a.OrdNo,
                           PartyName = a.SubAccName,
                           POQty = a.poqty,
                           RecQty = d.Received,
                           ReqQty = a.reqqty,
                           Balance = sub.qtyp,
                           LocId = a.LocID
                       };
            return data.ToList();
        }
    }

    
}

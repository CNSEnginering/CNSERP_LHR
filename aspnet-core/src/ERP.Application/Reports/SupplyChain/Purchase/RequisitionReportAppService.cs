using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.SupplyChain.Purchase;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class RequisitionReportAppService : ERPReportAppServiceBase , IRequisitionReportAppService
    {
        private IRepository<Requisitions> _requisitionRepository;
        private IRepository<RequisitionDetail> _requisitionDetailRepository;
        private IRepository<ICItem> _itemRepository;
        private IRepository<ICLocation> _locRepository;
        private IRepository<CostCenter> _costCenterRepository;
        private IRepository<SubCostCenter> _subCostCenterRepository;
        public RequisitionReportAppService(IRepository<Requisitions> requisitionRepository,
          IRepository<RequisitionDetail> requisitionDetailRepository,
          IRepository<ICItem> itemRepository,
          IRepository<ICLocation> locRepository,
          IRepository<CostCenter> costCenterRepository,
          IRepository<SubCostCenter> subCostCenterRepository)
        {
            _requisitionRepository = requisitionRepository;
            _requisitionDetailRepository = requisitionDetailRepository;
            _itemRepository = itemRepository;
            _locRepository = locRepository;
            _costCenterRepository = costCenterRepository;
            _subCostCenterRepository = subCostCenterRepository;
        }
        public List<RequisitionReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _requisitionRepository.GetAll().Where(a => a.TenantId == tenantId)
                       join
                       b in _requisitionDetailRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.DocNo } equals new { A = b.TenantId, B = b.DocNo.Value }
                       join
                       d in _locRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = Convert.ToInt32(a.LocID), B = a.TenantId } equals new { A = d.LocID, B = d.TenantId }
                       join
                       e in _costCenterRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.CCID, B = a.TenantId } equals new { A = e.CCID, B = e.TenantId } into cs
                       from costsub in cs.DefaultIfEmpty()
                       join
                       f in _itemRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = b.ItemID, B = b.TenantId } equals new { A = f.ItemId, B = f.TenantId }
                       join
                       g in _subCostCenterRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = b.SUBCCID, B = a.TenantId, C = a.CCID } equals new { A = g.SUBCCID, B = g.TenantId, C = g.CCID } into sub
                       from subCost in sub.DefaultIfEmpty()

                       where (a.DocNo >= fromDoc && a.DocNo <= toDoc && a.TenantId == tenantId
                      // && a.TypeID == typeId
                      )
                       select new RequisitionReport()
                       {
                           Unit = f.StockUnit,
                           ItemId = b.ItemID,
                           Descp = f.Descp,
                           Qty = b.Qty,
                           Remarks = b.Remarks,
                           DocNo = a.DocNo,
                           DocDate = string.Format("{0:dd/MM/yyyy}", a.DocDate),
                           Id = a.Id,
                           QIH = b.QIH,
                           LocName = d.LocName,
                           CostCenterName = costsub.CCName,
                           SubCostCenterName = subCost.SubCCName,
                           ReqNo = a.ReqNo,
                           Narration = a.Narration,
                           TransId = b.TransId,
                           TransName = b.TransName,
                           PartyName = a.PartyName,
                           ItemName = a.ItemName,
                           OrderQty = a.OrderQty,
                           BasicStyle=a.BasicStyle,
                           License=a.License,
                           JobOrderNo = a.OrdNo,
                           ExpectedArrivalDate = string.Format("{0:dd/MM/yyyy}", a.ExpArrivalDate),

                       };
            return data.ToList();
        }
    }

    
}

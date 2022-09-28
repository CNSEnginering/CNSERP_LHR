using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.SupplyChain.Purchase;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase.ReceiptEntry;
using ERP.SupplyChain.Purchase.ReceiptReturn;
using ERP.SupplyChain.Purchase.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ReceiptReturnReportAppService : ERPReportAppServiceBase , IReceiptReturnReportAppService
    {
        private IRepository<ICItem> _itemRepository;
        private IRepository<ICLocation> _locRepository;
        private IRepository<PORETHeader> _receiptHeaderRepository;
        private IRepository<PORETDetail> _receiptDetailRepository;
        private IRepository<ChartofControl, string> _accRepository;
        private IRepository<AccountSubLedger> _accSubRepository;
        private IRepository<CostCenter> _ccRepository;
        public ReceiptReturnReportAppService(
          IRepository<ICItem> itemRepository,
          IRepository<ICLocation> locRepository,
          IRepository<PORETHeader> receiptHeaderRepository,
          IRepository<PORETDetail> receiptDetailRepository,
          IRepository<ChartofControl, string> accRepository,
          IRepository<AccountSubLedger> accSubRepository,
          IRepository<CostCenter> ccRepository)
        {
            _receiptHeaderRepository = receiptHeaderRepository;
            _receiptDetailRepository = receiptDetailRepository;
            _itemRepository = itemRepository;
            _locRepository = locRepository;
            _accRepository = accRepository;
            _accSubRepository = accSubRepository;
            _ccRepository = ccRepository;
        }
        public List<ReceiptReturnReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _receiptHeaderRepository.GetAll().Where(a => a.TenantId == tenantId)
                       join
                       b in _receiptDetailRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.DocNo } equals new { A = b.TenantId, B = b.DocNo }
                       join
                       c in _receiptDetailRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.Id } equals new { A = c.TenantId, B = c.DetID }
                       join
                       d in _locRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = Convert.ToInt32(a.LocID), B = a.TenantId } equals new { A = d.LocID, B = d.TenantId }
                       join
                       e in _accRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.AccountID, B = a.TenantId } equals new { A = e.Id, B = e.TenantId }
                       join
                       f in _itemRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = b.ItemID, B = b.TenantId } equals new { A = f.ItemId, B = f.TenantId }
                       join
                       g in _accSubRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.AccountID, B = a.TenantId, C = Convert.ToInt32(a.SubAccID) } equals new { A = g.AccountID, B = g.TenantId, C = g.Id }
                       //join
                       //h in _ccRepository.GetAll().Where(a => a.TenantId == tenantId)
                       //on new { A = a.CCID, B = a.TenantId} equals new { A = h.CCID, B = h.TenantId }
                       where (a.DocNo >= fromDoc && a.DocNo <= toDoc && a.TenantId == tenantId)
                       select new ReceiptReturnReport()
                       {
                           Unit = b.Unit,
                           ItemId = b.ItemID,
                           Descp = f.Descp,
                           Qty = b.Qty,
                           Remarks = b.Remarks,
                           DocNo = a.DocNo,
                           DocDate = a.DocDate.ToString(),
                           Id = a.Id,
                           LocName = d.LocName,
                           Narration = a.Narration,
                           AccId = e.Id,
                           SubAccId = g.Id,
                           AccName = e.AccountName,
                           SubAccName = g.SubAccName,
                           Amount = b.Amount,
                           BillAmount = a.BillAmt,
                           BillDate = (a.BillDate.Value.Year + "/" + a.BillDate.Value.Month + "/" + a.BillDate.Value.Day).ToString(),
                           BillNo = a.BillNo,
                           Rate = b.Rate,
                           TaxRate = b.TaxRate,
                           TaxAmount = b.TaxAmt,
                           NetAmount = b.NetAmount,
                           //PONo = a.PODocNo,
                           IGPNo = a.IGPNo,
                           OrdNo = a.OrdNo,
                           RecDocNo = a.RecDocNo,
                           Freight = a.Freight,
                           AddFreight = a.AddFreight,
                           AddDisc = a.AddDisc,
                           AddLeak = a.AddLeak,
                           AddExp = a.AddExp
                           //CCID = a.CCID,
                           //CostCenterName = h.CCName
                       };
            return data.ToList();
        }
    }

    
}

using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.Reports.SupplyChain.Purchase;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase.PurchaseOrder;
using ERP.SupplyChain.Purchase.ReceiptEntry;
using ERP.SupplyChain.Purchase.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ReceiptReportAppService : ERPReportAppServiceBase, IReceiptReportAppService
    {
        private IRepository<ICItem> _itemRepository;
        private IRepository<ICLocation> _locRepository;
        private IRepository<PORECHeader> _receiptHeaderRepository;
        private IRepository<PORECDetail> _receiptDetailRepository;
        private IRepository<ChartofControl, string> _accRepository;
        private IRepository<AccountSubLedger> _accSubRepository;
        private IRepository<CostCenter> _ccRepository;
        private IRepository<POPOHeader> _popoRepository;
        private IRepository<GLTRHeader> _gltrHeader;

        public ReceiptReportAppService(
          IRepository<ICItem> itemRepository,
          IRepository<ICLocation> locRepository,
          IRepository<PORECHeader> receiptHeaderRepository,
          IRepository<PORECDetail> receiptDetailRepository,
          IRepository<ChartofControl, string> accRepository,
          IRepository<AccountSubLedger> accSubRepository,
          IRepository<CostCenter> ccRepository,
           IRepository<POPOHeader> popoRepository,
            IRepository<GLTRHeader> gltrHeader)
        {
            _receiptHeaderRepository = receiptHeaderRepository;
            _receiptDetailRepository = receiptDetailRepository;
            _itemRepository = itemRepository;
            _locRepository = locRepository;
            _accRepository = accRepository;
            _accSubRepository = accSubRepository;
            _ccRepository = ccRepository;
            _popoRepository = popoRepository;
            _gltrHeader = gltrHeader;
        }
        public List<ReceiptReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _receiptHeaderRepository.GetAll().Where(a => a.TenantId == tenantId)
                       join
                       b in _receiptDetailRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.DocNo } equals new { A = b.TenantId, B = b.DocNo }
                       join
                       c in _popoRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = Convert.ToInt32(a.PODocNo) } equals new { A = c.TenantId, B = c.DocNo }
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
                       join
                       h in _gltrHeader.GetAll().Where(a => a.TenantId == AbpSession.TenantId)
                       on new { A = Convert.ToInt32(a.LinkDetID), B = a.TenantId } equals new { A = h.Id, B = h.TenantId } into ps
                       from h in ps.DefaultIfEmpty()
                           //join
                           //h in _ccRepository.GetAll().Where(a => a.TenantId == tenantId)
                           //on new { A = a.CCID, B = a.TenantId} equals new { A = h.CCID, B = h.TenantId }
                       where (a.DocNo >= fromDoc && a.DocNo <= toDoc && a.TenantId == tenantId)
                       select new ReceiptReport()
                       {
                           Unit = b.Unit,
                           ItemId = b.ItemID,
                           Descp = f.Descp,
                           Qty = b.Qty,
                           Remarks = b.Remarks,
                           DocNo = a.DocNo,
                           DocDate = string.Format("{0:dd/MM/yyyy}", a.DocDate),
                           Id = a.Id,
                           LocName = d.LocName,
                           Narration = a.Narration,
                           AccId = e.Id,
                           SubAccId = g.Id,
                           AccName = e.AccountName,
                           SubAccName = g.SubAccName,
                           Amount = b.Amount,
                           BillAmount = a.BillAmt,
                           BillDate = string.Format("{0:dd/MM/yyyy}", a.BillDate),
                           BillNo = a.BillNo,
                           Rate = b.Rate,
                           TaxRate = b.TaxRate,
                           TaxAmount = b.TaxAmt,
                           NetAmount = b.NetAmount,
                           PONo = a.PODocNo,
                           IGPNo = a.IGPNo,
                           OrdNo = a.OrdNo,
                           RecDocNo = a.RecDocNo,
                           Freight = a.Freight,
                           AddFreight = a.AddFreight,
                           AddDisc = a.AddDisc,
                           AddLeak = a.AddLeak,
                           AddExp = a.AddExp,
                           PODocDate = string.Format("{0:dd/MM/yyyy}", c.DocDate),
                           PurchaseVoucherNo = h.BookID + h.DocNo
                           //CCID = a.CCID,
                           //CostCenterName = h.CCName
                       };
            return data.ToList();
        }
    }


}

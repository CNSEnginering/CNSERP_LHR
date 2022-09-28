using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.SupplyChain.Purchase;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase.PurchaseOrder;
using ERP.SupplyChain.Purchase.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class PurchaseOrderReportAppService : ERPReportAppServiceBase , IPurchaseOrderReportAppService
    {
        private IRepository<POPOHeader> _popoHeaderRepository;
        private IRepository<POPODetail> _popoDetailRepository;
        private IRepository<ICItem> _itemRepository;
        private IRepository<ICLocation> _locRepository;
        private IRepository<ChartofControl, string> _accountRepository;
        private IRepository<AccountSubLedger> _accountSubRepository;
        public PurchaseOrderReportAppService(IRepository<POPOHeader> popoHeaderRepository,
            IRepository<POPODetail> popoDetailRepository,
            IRepository<ICItem> itemRepository,
            IRepository<ICLocation> locRepository,
            IRepository<ChartofControl, string> accountRepository,
            IRepository<AccountSubLedger> accountSubRepository)
        {
            _popoHeaderRepository = popoHeaderRepository;
            _popoDetailRepository = popoDetailRepository;
            _itemRepository = itemRepository;
            _locRepository = locRepository;
            _accountRepository = accountRepository;
            _accountSubRepository = accountSubRepository;
        }
        public List<PurchaseOrderReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId, string exfromdate, string extodate)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _popoHeaderRepository.GetAll().Where(a => a.TenantId == tenantId)
                       join
                       b in _popoDetailRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.DocNo } equals new { A = b.TenantId, B = b.DocNo }
                       join
                       d in _locRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = Convert.ToInt32(a.LocID), B = a.TenantId } equals new { A = d.LocID, B = d.TenantId }
                       join
                       e in _accountRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.AccountID, B = a.TenantId } equals new { A = e.Id, B = e.TenantId }
                       join
                       f in _itemRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = b.ItemID, B = b.TenantId } equals new { A = f.ItemId, B = f.TenantId }
                       join
                        g in _accountSubRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = Convert.ToInt32(a.SubAccID), B = a.TenantId, C = a.AccountID } equals new { A = g.Id, B = g.TenantId, C = g.AccountID }
                       into accsub
                       from sub in accsub.DefaultIfEmpty()
                       where (a.DocNo >= fromDoc && a.DocNo <= toDoc && a.TenantId == tenantId
                       //&& a.ArrivalDate.Value.Date >= Convert.ToDateTime(exfromdate).Date
                       //&& a.ArrivalDate.Value.Date <= Convert.ToDateTime(extodate).Date
                      // && a.TypeID == typeId
                      )
                       select new PurchaseOrderReport()
                       {
                           Unit = b.Unit,
                           ItemId = b.ItemID,
                           Descp = f.Descp,
                           Qty = b.Qty,
                           Remarks = a.Narration,
                           DocNo = a.DocNo,
                           DocDate = a.DocDate,
                           Id = a.Id,
                           LocName = d.LocName,
                           AccName = e.AccountName,
                           SubAccName = sub.SubAccName,
                           ReqNo = a.ReqNo,
                           Amount = b.Amount,
                           ArrivalDate=a.ArrivalDate,
                           Rate = b.Rate,
                           TotalTaxAmount = a.TaxAmount,
                           TotalTaxRate = a.TaxRate,
                           TaxRate = b.TaxRate,
                           TaxAmount = b.TaxAmt,
                           NetAmount = b.NetAmount  ,
                           Address=sub.Address1,
                           ContactPerson=sub.Contact,
                           PhoneNo=sub.Phone,
                           TermCondition=a.Terms,
                           
                       };
            return data.ToList();
        }
    }


}

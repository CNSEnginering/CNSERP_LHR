using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ItemLedgerAppService : ERPReportAppServiceBase
    {
         private IRepository<ICLEDG> _icledgRepository;
        private IRepository<ICItem> _icitemRepository;
        private IRepository<ICLocation> _locRepository;
        public ItemLedgerAppService(IRepository<ICLEDG> icledgRepository,
            IRepository<ICItem> icitemRepository,
            IRepository<ICLocation> locRepository)
        {
            _icledgRepository = icledgRepository;
            _icitemRepository = icitemRepository;
            _locRepository = locRepository;
        }

        public List<ItemLedger> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var itemledgerData = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
                                 join
                                 b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
                                 on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
                                 join
                                 c in _locRepository.GetAll().Where(o => o.TenantId == tenantId)
                                 on new { A = Convert.ToInt32(a.LocID), B = a.TenantId } equals new { A = c.LocID, B = c.TenantId }
                                 where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0 && Convert.ToDateTime(a.DocDate) >= Convert.ToDateTime(fromDate)
                                 && Convert.ToDateTime(a.DocDate) <= Convert.ToDateTime(toDate) && a.LocID >= Convert.ToInt32(fromLocId) && a.LocID <=
                                 Convert.ToInt32(toLocId) && a.TenantId == tenantId)
                                 orderby a.DocDesc
                                 select new ItemLedger()
                                 {
                                     DocType = a.DocDesc,
                                     DocDate = a.DocDate.Value.Date,
                                     locId = a.LocID,
                                     DocNo = a.DocNo,
                                     Desc = a.Descp,
                                     SrNo = a.srno,
                                     Receipt = (a.Qty > 0 ? a.Qty : 0),
                                     Issue = (a.Qty < 0 ? a.Qty : 0),
                                     ItemId = a.ItemID,
                                     locDesc = c.LocName,
                                     ItemDescp = b.Descp
                                 };
            return itemledgerData.ToList();
        }
    }
    public class ItemLedger
    {
        public string DocType { get; set; }
        public int? locId { get; set; }
        public string locDesc { get; set; }
        public string ItemId { get; set; }
        public string ItemDescp { get; set; }
        public DateTime DocDate { get; set; }
        public int DocNo { get; set; }
        public string Desc { get; set; }
        public string SrNo { get; set; }
        public double? Receipt { get; set; }
        public double? Issue { get; set; }
    }

}

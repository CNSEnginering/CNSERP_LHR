using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ItemLedgerDetailAppService : ERPReportAppServiceBase, IItemLedgerDetailAppService
    {
        private IRepository<ICLEDG> _icledgRepository;
        private IRepository<ICItem> _icitemRepository;
        private IRepository<ICLocation> _locRepository;
        public ItemLedgerDetailAppService(
            IRepository<ICLEDG> icledgRepository,
            IRepository<ICItem> icitemRepository,
            IRepository<ICLocation> locRepository)
        {
            _icledgRepository = icledgRepository;
            _icitemRepository = icitemRepository;
            _locRepository = locRepository;
        }

        public List<ItemLedgerdetail> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            List<int> LocIds = new List<int>();
            if (fromLocId!="")
            {
             LocIds = fromLocId.Split(',').Select(int.Parse).ToList();
            }

            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            IQueryable<ItemLedgerdetail> itemledgerData = null;

            itemledgerData = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
                             join
                             b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
                             on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
                             join
                             c in _locRepository.GetAll().Where(o => o.TenantId == tenantId)
                             on new { A = Convert.ToInt32(a.LocID), B = a.TenantId } equals new { A = c.LocID, B = c.TenantId }
                             where (
                             a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0 &&
                             Convert.ToDateTime(a.DocDate).Date >= Convert.ToDateTime(fromDate).Date
                             && Convert.ToDateTime(a.DocDate).Date <= Convert.ToDateTime(toDate).Date
                            &&
                            LocIds.Contains((int)a.LocID)
                             //a.LocID >= Convert.ToInt32(fromLocId)
                             //&&
                             //a.LocID <=
                             //Convert.ToInt32(toLocId)
                             &&
                             a.TenantId == tenantId
                             )
                             // orderby a.DocDate
                             //, a.DocType, a.DocNo
                             select new ItemLedgerdetail()
                             {
                                 DocType = a.DocDesc,
                                 DocDate = a.DocDate.Value.Date,
                                 locId = a.LocID,
                                 SortId = a.SortId,
                                 DocNo = a.DocNo,
                                 TypeID = a.DocType,
                                 Desc = a.Descp,
                                 SrNo = a.srno,
                                 Unit = a.UNIT,
                                 Receipt = (a.Qty > 0 ? a.Qty : 0),
                                 ReceiptRate = (a.Qty > 0 ? a.Rate : 0),
                                 ReceiptAmount = (a.Qty > 0 ? Math.Round(Convert.ToDouble(a.Amount), 2) : 0),
                                 Issue = (a.Qty < 0 ? a.Qty : 0) == null ? 0 : (a.Qty < 0 ? a.Qty : 0),
                                 IssueRate = (a.Qty < 0 ? a.Rate : 0) == null ? 0 : (a.Qty < 0 ? a.Rate : 0),
                                 IssueAmount = a.Qty < 0 ? Math.Round(Convert.ToDouble(a.Amount), 2) : 0,
                                 ItemId = a.ItemID,
                                 Seg1 = Convert.ToInt32(a.ItemID.Split('-', StringSplitOptions.None)[0]),
                                 locDesc = c.LocName,
                                 ItemDescp = b.Descp,
                             };
            //return itemledgerData.ToList();

            return itemledgerData.OrderBy(c => c.ItemId).ToList();
        }

    }


}

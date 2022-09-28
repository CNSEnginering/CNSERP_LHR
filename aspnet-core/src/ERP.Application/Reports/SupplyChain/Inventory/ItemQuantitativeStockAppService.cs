using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.IC_Segment3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ItemQuantitativeStockAppService : ERPReportAppServiceBase, IItemQuantitativeStockAppService
    {
        private IRepository<ICLEDG> _icledgRepository;
        private IRepository<ICItem> _icitemRepository;
        private IRepository<ICLocation> _locRepository;
        private IRepository<ICSegment3> _ICSeg3Repository;
        public ItemQuantitativeStockAppService(
            IRepository<ICLEDG> icledgRepository,
            IRepository<ICItem> icitemRepository,
            IRepository<ICLocation> locRepository,
            IRepository<ICSegment3> ICSeg3Repository)
        {
            _icledgRepository = icledgRepository;
            _icitemRepository = icitemRepository;
            _locRepository = locRepository;
            _ICSeg3Repository = ICSeg3Repository;
        }
        public List<ItemQuantitative> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem)
        {
            List<int> LocIds = new List<int>();
            if (fromLocId != "")
            {
                LocIds = fromLocId.Split(',').Select(int.Parse).ToList();
            }
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var itemledgerData = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId )
                                 join
                                 b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
                                 on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
                                 join
                                 c in _locRepository.GetAll().Where(o => o.TenantId == tenantId)
                                 on new { A = Convert.ToInt32(a.LocID), B = a.TenantId } equals new { A = c.LocID, B = c.TenantId }
                                 where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0
                                 && LocIds.Contains((int)a.LocID)
                                 && a.TenantId == tenantId)                             
                                 group new { a.ItemID, a.LocID, a.Qty, b.Descp, c.LocName ,b.StockUnit}
                                 by new { a.ItemID, a.LocID, b.Descp, c.LocName ,b.StockUnit} into g
                                 select new ItemQuantitative()
                                 {
                                     Qty = g.Sum(q => q.Qty),
                                     ItemId = g.Key.ItemID,
                                     locId = g.Key.LocID,
                                     locDesc = g.Key.LocName.ToUpper(),
                                     ItemDescp = g.Key.Descp,
                                     //DocType = a.DocDesc,
                                     //DocDate = a.DocDate.Value.Date,
                                     //locId = a.LocID,
                                     //DocNo = a.DocNo,
                                     //Desc = a.Descp,
                                     //SrNo = a.srno,
                                     //ItemId = a.ItemID,
                                     //locDesc = c.LocName.ToUpper(),
                                     //ItemDescp = b.Descp,
                                     //Qty = a.Qty,
                                     Unit = g.Key.StockUnit
                                 };
            return itemledgerData.ToList();
        }

        public List<ItemQuantitative> GetData3(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem)
        {
            List<int> LocIds = new List<int>();
            if (fromLocId != "")
            {
                LocIds = fromLocId.Split(',').Select(int.Parse).ToList();
            }
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var itemledgerData = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
                                 join
                                 b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
                                 on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
                                 join
                                 c in _locRepository.GetAll().Where(o => o.TenantId == tenantId)
                                 on new { A = Convert.ToInt32(a.LocID), B = a.TenantId } equals new { A = c.LocID, B = c.TenantId }
                                 join
                                 d in _ICSeg3Repository.GetAll().Where(o => o.TenantId == tenantId)
                                 on new { A = b.Seg3Id, B = b.TenantId } equals new { A = d.Seg3Id, B = d.TenantId }
                                 where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0
                                 && LocIds.Contains((int)a.LocID)
                                 && a.TenantId == tenantId)
                                 group new { b.Seg3Id,d.Seg3Name, a.LocID, a.Qty, c.LocName }
                                 by new { b.Seg3Id, a.LocID, c.LocName,d.Seg3Name, } into g
                                 select new ItemQuantitative()
                                 {
                                     Qty = g.Sum(q => q.Qty),
                                     ItemId = g.Key.Seg3Id,
                                     locId = g.Key.LocID,
                                     locDesc = g.Key.LocName.ToUpper(),
                                     ItemDescp = g.Key.Seg3Name
                                     //DocType = a.DocDesc,
                                     //DocDate = a.DocDate.Value.Date,
                                     //locId = a.LocID,
                                     //DocNo = a.DocNo,
                                     //Desc = a.Descp,
                                     //SrNo = a.srno,
                                     //ItemId = a.ItemID,
                                     //locDesc = c.LocName.ToUpper(),
                                     //ItemDescp = b.Descp,
                                     //Qty = a.Qty,
                                     //Unit = a.UNIT
                                 };
            return itemledgerData.ToList();
        }
    }

}

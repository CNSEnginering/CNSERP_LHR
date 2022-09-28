using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class TransferRegisterAppService : ERPReportAppServiceBase, ITransferRegisterAppService
    {
        private IRepository<Transfer> _transferRepository;
        private IRepository<TransferDetail> _transferDetailRepository;
        private IRepository<ICItem> _itemRepository;
        private IRepository<ICLocation> _locRepository;
        public TransferRegisterAppService(
          IRepository<ICItem> itemRepository,
          IRepository<ICLocation> locRepository,
          IRepository<Transfer> transferRepository,
          IRepository<TransferDetail> transferDetailRepository
          )
        {
            _itemRepository = itemRepository;
            _locRepository = locRepository;
            _transferRepository = transferRepository;
            _transferDetailRepository = transferDetailRepository;
        }
        public List<TransferRegister> GetData(int tenantId,
            int fromDoc, int toDoc, DateTime fromDate, DateTime toDate, string fromLoc, string toLoc)
        {
            List<int> LocIds = new List<int>();
            if (fromLoc != "")
            {
                LocIds = fromLoc.Split(',').Select(int.Parse).ToList();
            }
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _transferRepository.GetAll().Where(a => a.TenantId == tenantId)
                       join
                       b in _transferDetailRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                       join
                       e in _itemRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = b.ItemId, B = b.TenantId } equals new { A = e.ItemId, B = e.TenantId }
                       where (a.DocNo >= fromDoc && a.DocNo <= toDoc && a.TenantId == tenantId &&
                       a.DocDate.Date >= fromDate.Date && a.DocDate.Date <= toDate.Date
                       &&
                       //a.FromLocID >= fromLoc && a.ToLocID <= toLoc
                        LocIds.Contains((int)a.FromLocID)
                      )
                       select new TransferRegister()
                       {
                           Unit = b.Unit,
                           ItemId = b.ItemId,
                           ItemDescp = e.Descp,
                           Qty = b.Qty,
                           DocNo = a.DocNo,
                           Amount = b.Amount,
                           Description = a.Narration,
                           PoRef = a.OrdNo,
                           FromLocId = a.FromLocID,
                           ToLocId = a.ToLocID,
                           Cost = b.Cost,

                           FromLocName = _locRepository.GetAll().Where(o => o.LocID == a.FromLocID).Count() > 0 ?
                                         _locRepository.GetAll().Where(o => o.LocID == a.FromLocID).FirstOrDefault().LocName : "",
                           ToLocName = _locRepository.GetAll().Where(o => o.LocID == a.ToLocID).Count() > 0 ?
                                         _locRepository.GetAll().Where(o => o.LocID == a.ToLocID).FirstOrDefault().LocName : ""
                           ,
                           DocDate = (a.DocDate.Date.Day + "/" + a.DocDate.Date.Month + "/" + a.DocDate.Date.Year).ToString()


                       };
            return data.ToList();
        }
    }


}

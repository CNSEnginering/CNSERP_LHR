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
    public class StockTransferAppService : ERPReportAppServiceBase, IStockTransferAppService
    {
        private IRepository<Transfer> _transferRepository;
        private IRepository<TransferDetail> _transferDetailRepository;
        private IRepository<ICItem> _itemRepository;
        private IRepository<ICLocation> _locRepository;
        public StockTransferAppService(
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
        public List<StockTransfer> GetData(int tenantId,
            int fromDoc, int toDoc)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _transferRepository.GetAll().Where(a => a.TenantId == tenantId)
                       join
                       b in _transferDetailRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.DocNo } equals new { A = b.TenantId, B = b.DocNo }
                       join
                       e in _itemRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = b.ItemId, B = b.TenantId } equals new { A = e.ItemId, B = e.TenantId }
                       where (a.DocNo >= fromDoc && a.DocNo <= toDoc && a.TenantId == tenantId
                      )
                       select new StockTransfer()
                       {
                           Unit = b.Unit,
                           ItemId = b.ItemId,
                           Descp = e.Descp,
                           Qty = b.Qty,
                           DocNo = a.DocNo,
                           FromLocId = a.FromLocID,
                           ToLocId = a.ToLocID,
                           FromLocName = _locRepository.GetAll().Where(o => o.LocID == a.FromLocID && o.TenantId == tenantId).Count() > 0 ?
                                         _locRepository.GetAll().Where(o => o.LocID == a.FromLocID && o.TenantId == tenantId).FirstOrDefault().LocName : "",
                           ToLocName = _locRepository.GetAll().Where(o => o.LocID == a.ToLocID && o.TenantId == tenantId).Count() > 0 ?
                                         _locRepository.GetAll().Where(o => o.LocID == a.ToLocID && o.TenantId == tenantId).FirstOrDefault().LocName : ""
                           ,
                           DocDate = a.DocDate.Day.ToString() + "/" + a.DocDate.Month.ToString() + "/" + a.DocDate.Year.ToString()

                       };
            return data.ToList();
        }
    }


}

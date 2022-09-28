using Abp.Domain.Repositories;
using ERP.Reports.SupplyChain.Purchase;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class PurchaseOrderStatusReportAppService : ERPReportAppServiceBase, IPurchaseOrderStatusReportAppService
    {
        private readonly IRepository<POSTAT> _repository;
        private readonly IRepository<ICItem> _icitemRepository;
        public PurchaseOrderStatusReportAppService(
            IRepository<POSTAT> repository,
            IRepository<ICItem> icitemRepository)
        {
            _repository = repository;
            _icitemRepository = icitemRepository;
        }
        public List<PurchaseOrderStatus> GetData(int tenantId, int fromDoc, int toDoc, string fromDate, string toDate, string fromArDate, string toArDate)
        {

            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _repository.GetAll().Where(o => o.TenantId == tenantId)
                       join
                       b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
                       where (a.TenantId == tenantId && a.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                       && a.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && a.ArrivalDate.Value.Date >= Convert.ToDateTime(fromArDate).Date
                       && a.ArrivalDate.Value.Date <= Convert.ToDateTime(toArDate).Date &&
                       a.DocNo >= fromDoc && a.DocNo <= toDoc
                       )
                       select new PurchaseOrderStatus()
                       {
                           Party = a.LedgerDesc + "-" + a.SubAccName,
                           DocNo = a.DocNo.ToString(),
                           ItemId = b.ItemId,
                           Convr = b.Conver,
                           ItemDesc = b.Descp,
                           Qty = a.Qty,
                           Rate = a.Rate,
                           Amount = a.Amount,
                           RecQty = a.Received,
                           RetQty = a.Returned,
                           Unit = a.Unit,
                           Balance = a.Qty - (a.Received + a.Returned),
                           BalAmt = a.QtyPAmt
                       };


            return data.ToList();
        }
    }

}

using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class StockAssemblyRegisterAppService : ERPReportAppServiceBase , IStockAssemblyRegisterAppService
    {
        private IRepository<Assembly> _assemblyHeadRepository;
        private IRepository<AssemblyDetails> _assemblyDetailsRepository;
        private IRepository<ICItem> _itemRepository;
        public StockAssemblyRegisterAppService(IRepository<Assembly> assemblyHeadRepository,
            IRepository<AssemblyDetails> assemblyDetailsRepository,
            IRepository<ICItem> itemRepository
            )
        {
            _assemblyHeadRepository = assemblyHeadRepository;
            _assemblyDetailsRepository = assemblyDetailsRepository;
            _itemRepository = itemRepository;
        }
        public List<AssemblyStockRegister> GetData(int? tenantId, string fromDate, string toDate, int fromDoc, int toDoc)
        {
            tenantId = tenantId == 0 ? AbpSession.TenantId : tenantId;
            var data = from a in _assemblyHeadRepository.GetAll().Where(o => o.TenantId == tenantId)
                       join
                       b in _assemblyDetailsRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                       join c in _itemRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = b.TenantId, B = b.ItemID } equals new { A = c.TenantId, B = c.ItemId }
                       where (a.TenantId == tenantId
                                && a.DocDate >= Convert.ToDateTime(fromDate) && a.DocDate <= Convert.ToDateTime(toDate)
                                && a.DocNo >= fromDoc
                                && a.DocNo <= toDoc
                                )
                       select new AssemblyStockRegister()
                       {
                           ItemId = b.ItemID,
                           ItemName = c.Descp,
                           Convr = b.Conver,
                           Narration = a.Narration,
                           Qty = b.Qty,
                           TransType = b.TransType == 7 ? "Product" : "Raw material",
                           Unit = b.Unit,
                           DocNo = a.DocNo,
                           LocID = a.LocID,
                           Amount = b.Amount,
                           DocDate = a.DocDate.Value.Day.ToString() +"/"+ a.DocDate.Value.Month.ToString() +"/"+ a.DocDate.Value.Year.ToString(),
                           OverHead = a.OverHead,
                           Rate = b.Rate,
                           Remarks = b.Remarks
                       };
            return data.ToList();
        }
    }

  
}

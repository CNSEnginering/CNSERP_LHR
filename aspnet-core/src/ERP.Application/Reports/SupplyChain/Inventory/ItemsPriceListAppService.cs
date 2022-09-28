using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ItemsPriceListAppService : ERPReportAppServiceBase , IItemsPriceListAppService
    {
        private readonly IRepository<ItemsPriceList> _itemsPriceRepository;
        public ItemsPriceListAppService(IRepository<ItemsPriceList> itemsPriceRepository)
        {
            _itemsPriceRepository = itemsPriceRepository;
        }
 

        public List<ItemsPriceListDto> GetData(int? TenantId, string priceList, string fromItem, string toItem)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var itemsPrice = _itemsPriceRepository.GetAll().Where(o => o.TenantId == TenantId && o.PriceList == priceList && o.ItemID.CompareTo(fromItem) >= 0 && o.ItemID.CompareTo(toItem)<=0);
            var data = from a in itemsPrice
                       orderby a.ItemID
                       select new ItemsPriceListDto()
                       {
                           PriceList = a.PriceList,
                           ItemID = a.ItemID,
                           Descp = a.Descp,
                           StockUnit = a.StockUnit,
                           PurPrice = a.PurPrice.ToString(),
                           SalesPrice = a.SalesPrice.ToString(),
                           DiscValue = a.DiscValue.ToString(),
                           NetPrice = a.NetPrice.ToString()

                       };
            return data.ToList();
        }
    }

   
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory.Dtos
{
    public class ItemsPriceListDto
    {
        public string PriceList { get; set; }

        public string ItemID { get; set; }

        public string Descp { get; set; }
        public string StockUnit { get; set; }
        public string PurPrice { get; set; }
        public string SalesPrice { get; set; }

        public string DiscValue { get; set; }

        public string NetPrice { get; set; }

    }
}

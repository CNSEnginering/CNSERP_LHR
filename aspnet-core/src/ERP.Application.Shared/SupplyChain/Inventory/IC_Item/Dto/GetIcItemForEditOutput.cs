using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Item.Dto
{
    public class GetIcItemForEditOutput
    {
        public CreateOrEditIcItemDto IcItem { get; set; }
        public string Seg1Name { get; set; }
        public string Seg2Name { get; set; }
        public string Seg3Name { get; set; }
        public string defCustomerAccDesc { get; set; }
        public string defVendorAccDesc { get; set; }
        public string Option4Desc { get; set; }
        public string Option5Desc { get; set; }
        public string DefTaxAuthDesc { get; set; }
    }
}

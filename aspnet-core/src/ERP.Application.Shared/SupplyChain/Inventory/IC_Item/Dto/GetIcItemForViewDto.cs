using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Item.Dto
{
    public class GetIcItemForViewDto
    {
        public IcItemDto IcItem { get; set; }
        public string Seg1Name { get; set; }
        public string Seg2Name { get; set; }
        public string Seg3Name { get; set; }
    }
}

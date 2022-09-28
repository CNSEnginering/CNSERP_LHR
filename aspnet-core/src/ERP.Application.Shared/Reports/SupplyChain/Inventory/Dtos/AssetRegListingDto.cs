using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory.Dtos
{
    public class AssetRegListingDto
    {
        public string AssetID { get; set; }

        public string AssetName { get; set; }

        public string RegDate { get; set; }
        public string AssetType { get; set; }
        public string LocID { get; set; }
        public string Location { get; set; }


    }
}

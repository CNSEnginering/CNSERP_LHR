using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Dto
{
    public class GetAllICSgment1ForExcelInput
    {
        public string Filter { get; set; }

        public string Seg1IDFilter { get; set; }
        public string Seg1NameFilter { get; set; }
    }
}

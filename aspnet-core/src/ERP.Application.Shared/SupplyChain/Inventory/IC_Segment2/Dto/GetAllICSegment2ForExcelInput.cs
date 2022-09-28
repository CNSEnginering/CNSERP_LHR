using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Dto
{
    public class GetAllICSegment2ForExcelInput
    {

        public string Filter { get; set; }
        public string Seg2IdFilter { get; set; }

        public string Seg2NameFilter { get; set; }

        public string Seg1IdFilter { get; set; }
    }
}

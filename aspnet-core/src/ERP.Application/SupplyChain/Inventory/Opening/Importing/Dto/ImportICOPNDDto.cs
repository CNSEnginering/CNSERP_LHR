using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Opening.Importing.Dto
{
  public  class ImportICOPNDDto
    {
        public int? ItemDocNo { get; set; }
        public string ItemID { get; set; }
        public string Unit { get; set; }
        public int? Conversion { get; set; }
        public int? Quantity { get; set; }
        public double? Rate { get; set; }
        public double? Amount { get; set; }
        public string Remarks { get; set; }
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Opening.Importing.Dto
{
    public class ImportICOPNHDto
    {
        public int TenantId { get; set; }
        public int? LocID { get; set; }
        public int? DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public string Narration { get; set; }  
        public int? OrderNo { get; set; }
        public List<ImportICOPNDDto> ICOPNDetail { get; set; }
        public bool? Active { get; set; }
        public bool? Approved { get; set; }
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}

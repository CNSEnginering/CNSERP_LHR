using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.InventoryGLLinks.Importing.Dtos
{
    public class ImportInventoryGLLinksDto
    {
        public int TenantId { get; set; }

        public int LocID { get; set; }
        public int GLLocID { get; set; }

        public string SegID { get; set; }
        public string LocName { get; set; }
        public string SegName { get; set; }
        public string AccRec { get; set; }

        public string AccRet { get; set; }

        public string AccAdj { get; set; }

        public string AccCGS { get; set; }

        public string AccWIP { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing IC Segemnt 3
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Importing.Dto
{
    public class ImportICSegment3Dto
    {
        public int Id { get; set; }
        [Required]
        public string Seg3Id { get; set; }

        public string Seg3Name { get; set; }

        public string Seg2Id { get; set; }

        public string Seg1Id { get; set; }
        public int TenantId { get; set; }

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

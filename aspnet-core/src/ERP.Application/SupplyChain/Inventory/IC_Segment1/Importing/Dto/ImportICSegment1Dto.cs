using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Importing.Dto
{
    public class ImportICSegment1Dto
    {
        public int Id { get; set; }
        [Required]
        public string Seg1ID { get; set; }
        [Required]
        public string Seg1Name { get; set; }
        public int TenantId { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing IC Segemnt 1
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}

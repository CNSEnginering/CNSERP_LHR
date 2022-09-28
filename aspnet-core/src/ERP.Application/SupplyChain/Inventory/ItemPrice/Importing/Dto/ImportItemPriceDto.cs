using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.SupplyChain.Inventory.ItemPrice.Importing.Dto
{
    public class ImportItemPriceDto
    {
        public int Id { get; set; }

        [Required]
        public virtual string PriceList { get; set; }

        [Required]
        public virtual string ItemID { get; set; }

        public virtual int priceType { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual double? DiscValue { get; set; }

        public virtual decimal? NetPrice { get; set; }

        public virtual short? Active { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }
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

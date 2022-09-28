using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.SupplyChain.Inventory
{
    [Table("ICTRAND")]
    public class TransferDetail : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public int DetID { get; set; }
        public int FromLocId { get; set; }
        public int DocNo { get; set; }
        public string ItemId { get; set; }
        public string Unit { get; set; }
        public decimal? Conver { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public string Bundle { get; set; }
        public string LotNo { get; set; }
    }
}

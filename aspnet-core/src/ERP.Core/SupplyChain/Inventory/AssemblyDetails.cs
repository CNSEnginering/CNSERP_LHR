using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.SupplyChain.Inventory
{
    [Table("ICASMD")]
    public class AssemblyDetails : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public int TransType { get; set; }
        public int DetID { get; set; }
        public int LocID { get; set; }
        public int DocNo { get; set; }
        public string ItemID { get; set; }
        public string Unit { get; set; }
        public decimal Conver { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}

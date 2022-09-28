using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment1
{
    [Table("ICSEG1")]
    public class ICSegment1 : Entity, IMustHaveTenant
    {
        [Required]
        public string Seg1ID { get; set; }
        [Required]
        public string Seg1Name { get; set; }
        public int TenantId { get; set; }

    }
}

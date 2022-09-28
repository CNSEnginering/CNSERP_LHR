using Abp.Domain.Entities;
using System;
using ERP.SupplyChain.Inventory.IC_Segment1;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.SupplyChain.Inventory.IC_Segment2
{
    [Table("ICSEG2")]
    public class ICSegment2 : Entity, IMustHaveTenant
    {
        public string Seg2Id { get; set; }

        public string Seg2Name { get; set; }

        public string Seg1Id { get; set; }

        public int TenantId { get; set; }
    }
}

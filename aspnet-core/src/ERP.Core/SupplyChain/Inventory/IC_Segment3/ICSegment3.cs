using System;
using Abp.Domain.Entities;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.SupplyChain.Inventory.IC_Segment3
{
    [Table("ICSEG3")]
    public class ICSegment3 : Entity, IMustHaveTenant
    {
        public string Seg3Id { get; set; }

        public string Seg3Name { get; set; }

        public string Seg2Id { get; set; }

        public string Seg1Id { get; set; }

        public int TenantId { get; set; }
    }
}

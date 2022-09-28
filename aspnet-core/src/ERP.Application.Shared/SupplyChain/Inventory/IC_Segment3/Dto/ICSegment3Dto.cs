using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Dto
{
    public class ICSegment3Dto : EntityDto
    {
        public string Seg3Id { get; set; }

        public string Seg3Name { get; set; }

        public string Seg2Id { get; set; }

        public string Seg1Id { get; set; }
    }
}

using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Dto
{
    public class ICSegment2Dto : EntityDto
    {
        public string Seg2Id { get; set; }

        public string Seg2Name { get; set; }

        public string Seg1Id { get; set; }

        public string Seg1Name { get; set; }

    }
}

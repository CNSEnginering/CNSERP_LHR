using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Dto
{
    public class CreateOrEditICSegment3Dto : EntityDto
    {
        public string Seg3Id { get; set; }

        public string Seg3Name { get; set; }

        public string Seg2Id { get; set; }

        public string Seg1Id { get; set; }

        public bool flag { get; set; }
    }
}

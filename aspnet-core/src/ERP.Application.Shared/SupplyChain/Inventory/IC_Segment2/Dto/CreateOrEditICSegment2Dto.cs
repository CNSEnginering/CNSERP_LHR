using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Dto
{
    public class CreateOrEditICSegment2Dto:EntityDto
    {
        public string Seg2Id { get; set; }

        public string Seg2Name { get; set; }

        public string Seg1Id { get; set; }

        public bool flag { get; set; }
    }
}

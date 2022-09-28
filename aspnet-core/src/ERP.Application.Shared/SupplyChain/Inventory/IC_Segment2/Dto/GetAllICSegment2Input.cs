using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Dto
{
    public class GetAllICSegment2Input: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string Seg2IdFilter { get; set; }

        public string Seg2NameFilter { get; set; }

        public string Seg1NameFilter { get; set; }
    }
}

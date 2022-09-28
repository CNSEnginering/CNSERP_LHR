using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Dto
{
    public class CreateOrEditICSegment1Dto: EntityDto<int?>
    {
        [Required]
        public string Seg1ID { get; set; }
        [Required]
        public string Seg1Name { get; set; }
        [Required]
        public bool flag { get; set; }
    }
}

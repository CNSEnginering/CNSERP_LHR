
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditICELocationDto : EntityDto<int?>
    {
		
		
		public int? TenantId { get; set; }
        public int? ParentId { get; set; }


        [Required]
		[StringLength(ICELocationConsts.MaxLocationTitleLength, MinimumLength = ICELocationConsts.MinLocationTitleLength)]
		public string LocationTitle { get; set; }
		
		
		[StringLength(ICELocationConsts.MaxAudtUserLength, MinimumLength = ICELocationConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(ICELocationConsts.MaxApprovedByLength, MinimumLength = ICELocationConsts.MinApprovedByLength)]
		public string ApprovedBy { get; set; }
		
		
		public DateTime? ApprovedDate { get; set; }
		
		

    }
}
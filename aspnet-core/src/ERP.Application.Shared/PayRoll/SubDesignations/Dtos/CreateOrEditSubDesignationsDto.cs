
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.SubDesignations.Dtos
{
    public class CreateOrEditSubDesignationsDto : EntityDto<int?>
    {

		[Required]
		public int SubDesignationID { get; set; }
		
		
		[StringLength(SubDesignationsConsts.MaxSubDesignationLength, MinimumLength = SubDesignationsConsts.MinSubDesignationLength)]
		public string SubDesignation { get; set; }
		
		
		public bool Active { get; set; }
		
		
		[StringLength(SubDesignationsConsts.MaxAudtUserLength, MinimumLength = SubDesignationsConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(SubDesignationsConsts.MaxCreatedByLength, MinimumLength = SubDesignationsConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
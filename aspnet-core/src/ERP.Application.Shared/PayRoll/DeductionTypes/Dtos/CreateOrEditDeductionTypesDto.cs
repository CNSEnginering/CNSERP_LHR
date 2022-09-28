
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.DeductionTypes.Dtos
{
    public class CreateOrEditDeductionTypesDto : EntityDto<int?>
    {

		[Required]
		public int TypeID { get; set; }
		
		
		[StringLength(DeductionTypesConsts.MaxTypeDescLength, MinimumLength = DeductionTypesConsts.MinTypeDescLength)]
		public string TypeDesc { get; set; }
		
		
		public bool Active { get; set; }
		
		
		[StringLength(DeductionTypesConsts.MaxAudtUserLength, MinimumLength = DeductionTypesConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(DeductionTypesConsts.MaxCreatedByLength, MinimumLength = DeductionTypesConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
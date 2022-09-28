
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EarningTypes.Dtos
{
    public class CreateOrEditEarningTypesDto : EntityDto<int?>
    {

		[Required]
		public int TypeID { get; set; }
		
		
		[StringLength(EarningTypesConsts.MaxTypeDescLength, MinimumLength = EarningTypesConsts.MinTypeDescLength)]
		public string TypeDesc { get; set; }
		
		
		public bool Active { get; set; }
		
		
		[StringLength(EarningTypesConsts.MaxAudtUserLength, MinimumLength = EarningTypesConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(EarningTypesConsts.MaxCreatedByLength, MinimumLength = EarningTypesConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
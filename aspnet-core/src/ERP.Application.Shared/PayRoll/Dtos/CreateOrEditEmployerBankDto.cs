
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Dtos
{
    public class CreateOrEditEmployerBankDto : EntityDto<int?>
    {

		[Required]
		public int EBankID { get; set; }
		
		
		[StringLength(EmployerBankConsts.MaxEBankNameLength, MinimumLength = EmployerBankConsts.MinEBankNameLength)]
		public string EBankName { get; set; }
		
		
		public bool Active { get; set; }
		
		
		[StringLength(EmployerBankConsts.MaxAudtUserLength, MinimumLength = EmployerBankConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(EmployerBankConsts.MaxCreatedByLength, MinimumLength = EmployerBankConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
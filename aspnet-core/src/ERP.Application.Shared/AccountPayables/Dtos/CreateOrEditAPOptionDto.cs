
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountPayables.Dtos
{
    public class CreateOrEditAPOptionDto : EntityDto<int?>
    {

		public string DEFBANKID { get; set; }
		
		
		public int? DEFPAYCODE { get; set; }
		
		
		public string DEFVENCTRLACC { get; set; }
		
		
		public string DEFCURRCODE { get; set; }
		
		
		public string PAYTERMS { get; set; }
		
		
		public DateTime? AUDTDATE { get; set; }
		
		
		public string AUDTUSER { get; set; }
		
		
		 //public string? CurrencyRateId { get; set; }
		 
		 //		 public int? BankId { get; set; }
		 
		 //		 public string? ChartofControlId { get; set; }
		 
		 
    }
}

using System;
using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.Dtos
{
    public class APOptionDto : EntityDto
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
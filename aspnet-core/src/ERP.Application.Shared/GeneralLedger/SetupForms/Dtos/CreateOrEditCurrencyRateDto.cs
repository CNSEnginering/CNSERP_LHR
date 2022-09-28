
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditCurrencyRateDto : EntityDto<string>
    {
		
	
		public DateTime AUDTDATE { get; set; }
		
		
	
		public string AUDTUSER { get; set; }
		
		
		[Required]
		public string CURNAME { get; set; }
		
		
		
		public string SYMBOL { get; set; }
		
		
		
		public DateTime RATEDATE { get; set; }
		
		
	
		public double CURRATE { get; set; }

        public  int Decimal { get; set; }

        public  string Narration { get; set; }
        public  string Unit { get; set; }

    }
}
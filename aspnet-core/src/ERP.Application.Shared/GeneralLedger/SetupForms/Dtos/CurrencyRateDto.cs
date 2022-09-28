
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CurrencyRateDto : EntityDto<string>
    {

		public DateTime AUDTDATE { get; set; }

		public string AUDTUSER { get; set; }

		public string CURNAME { get; set; }

		public string SYMBOL { get; set; }

		public DateTime RATEDATE { get; set; }

		public double CURRATE { get; set; }

		public string CMPID { get; set; }

        public string Narration { get; set; }
        public string Unit { get; set; }
    }
}
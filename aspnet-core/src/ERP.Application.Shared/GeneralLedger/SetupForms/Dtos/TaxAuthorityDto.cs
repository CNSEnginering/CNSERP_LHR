
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class TaxAuthorityDto : EntityDto<string>
    {
		

		public string CMPID { get; set; }

		public string TAXAUTHDESC { get; set; }

		public string ACCLIABILITY { get; set; }

		public string ACCRECOVERABLE { get; set; }

		public double RECOVERRATE { get; set; }

		public string ACCEXPENSE { get; set; }

		public DateTime AUDTDATE { get; set; }

		public string AUDTUSER { get; set; }


		 public string CompanyId { get; set; }

		 
    }
}
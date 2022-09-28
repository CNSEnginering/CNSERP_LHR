
using System;
using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.Setup.ApSetup.Dtos
{
    public class ApSetupDto : EntityDto
    {
		public int? DEFBANKID { get; set; }

		public int? DEFPAYCODE { get; set; }

		public string DEFVENCTRLACC { get; set; }

		public int? DEFCURRCODE { get; set; }
		public int? InventoryPoint { get; set; }
		public int? FinancePoint { get; set; }

		public DateTime? AUDTDATE { get; set; }

		public string AUDTUSER { get; set; }




    }
}
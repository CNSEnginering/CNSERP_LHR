
using System;
using Abp.Application.Services.Dto;

namespace ERP.AccountReceivables.RouteInvoices.Dtos
{
    public class ARINVDDto : EntityDto
    {
		public int DetID { get; set; }

		public string AccountID { get; set; }

		public int? SubAccID { get; set; }

		public int? DocNo { get; set; }

		public string InvNumber { get; set; }

		public double? InvAmount { get; set; }

		public double? TaxAmount { get; set; }

		public double? RecpAmount { get; set; }

		public string ChequeNo { get; set; }

		public bool Adjust { get; set; }

		public string Narration { get; set; }

        public string AccountName { get; set; }

        public string SubAccName { get; set; }



    }
}
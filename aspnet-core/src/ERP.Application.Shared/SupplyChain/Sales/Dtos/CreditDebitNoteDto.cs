
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.Dtos
{
    public class CreditDebitNoteDto : EntityDto
    {
		public int LocID { get; set; }
        public string LocName { get; set; }

        public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public DateTime? PostingDate { get; set; }

		public DateTime? PaymentDate { get; set; }

		public short? TypeID { get; set; }

		public string AccountID { get; set; }

		public int? SubAccID { get; set; }

		public string Reason { get; set; }

		public string Narration { get; set; }

		public string OGP { get; set; }

		public double? TotalQty { get; set; }

		public double? TotAmt { get; set; }

		public bool Posted { get; set; }

		public int? LinkDetID { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }
        public string TransType { get; set; }


    }
}
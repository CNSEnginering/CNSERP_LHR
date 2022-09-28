
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class GlChequeDto : EntityDto
    {
		public int? TenantID { get; set; }

		public int? DocID { get; set; }

		public int? TypeID { get; set; }

		public string EntryDate { get; set; }

		public string ChequeDate { get; set; }

		public string ChequeNo { get; set; }

		public double? ChequeAmt { get; set; }

		public string ChequeStatus { get; set; }

		public string PartyBank { get; set; }

		public string ChequeRef { get; set; }

		public string Remarks { get; set; }

		public int? LocationID { get; set; }

		public string AccountID { get; set; }

		public int? PartyID { get; set; }

        public string PartyName { get; set; }
        public string BankDesc { get; set; }

        public string BankID { get; set; }

		public bool Posted { get; set; }

		public string AUDTUSER { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }



    }
}
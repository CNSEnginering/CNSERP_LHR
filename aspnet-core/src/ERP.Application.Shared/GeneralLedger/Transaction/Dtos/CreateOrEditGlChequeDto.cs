
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class CreateOrEditGlChequeDto : EntityDto<int?>
    {

		public int TenantID { get; set; }
		
		
		public int? DocID { get; set; }
		
		
		public int? TypeID { get; set; }
		
		
		public DateTime? EntryDate { get; set; }
		public DateTime? StatusDate { get; set; }
		
		
		public DateTime? ChequeDate { get; set; }
		
		
		public string ChequeNo { get; set; }
		
		
		public double? ChequeAmt { get; set; }
		
		
		public string ChequeStatus { get; set; }
		
		
		public string PartyBank { get; set; }
		
		
		public string ChequeRef { get; set; }
		
		
		public string Remarks { get; set; }
		
		
		public int? LocationID { get; set; }
		
		
		public string AccountID { get; set; }
		
		
		public int? PartyID { get; set; }
		
		
		public string BankID { get; set; }
		
		
		public bool Posted { get; set; }
		
		
		public string AUDTUSER { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreatedDate { get; set; }

        public string LocDesc { get; set; }
        public string AccountName { get; set; }
        public string PartyName { get; set; }
        public string BankName { get; set; }
        public virtual byte? ChType { get; set; }
        public virtual string BankAccountID { get; set; }
        public virtual int ConfigID { get; set; }
        public int? LinkDetID { get; set; }
    }
}
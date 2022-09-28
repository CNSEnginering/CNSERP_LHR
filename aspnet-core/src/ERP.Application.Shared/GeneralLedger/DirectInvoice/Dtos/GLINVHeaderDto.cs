
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.DirectInvoice.Dtos
{
    public class GLINVHeaderDto : EntityDto<int?>
    {
		public int DocNo { get; set; }

		public string TypeID { get; set; }

        public int? LocID { get; set; }

        public string LocDesc { get; set; }

        public string PaymentOption { get; set; }

        public string BankID { get; set; }

		public string AccountID { get; set; }

		public int ConfigID { get; set; }

		public DateTime DocDate { get; set; }

        public string DocStatus { get; set; }

        public DateTime PostDate { get; set; }

		public string Narration { get; set; }

        public string CurID { get; set; }

        public double? CurRate { get; set; }

		public string ChequeNo { get; set; }

		public string RefNo { get; set; }

		public string PayReason { get; set; }

		public string PartyInvNo { get; set; }

        public string TaxAuth { get; set; }

        public string TaxAuthDesc { get; set; }

        public int? TaxClass { get; set; }

        public string TaxClassDesc { get; set; }

        public double? TaxRate { get; set; }

        public string TaxAccID { get; set; }

        public string TaxAccDesc { get; set; }

        public double? TaxAmount { get; set; }

        public double? ClosingBalance { get; set; }

        public double? CreditLimit { get; set; }

        public DateTime PartyInvDate { get; set; }
        public  bool? PostedStock { get; set; }
        public  string PostedStockBy { get; set; }
        public  DateTime? PostedStockDate { get; set; }

        public int? CprID { get; set; }
        public string CprNo { get; set; }
        public DateTime? CprDate { get; set; }

        public bool Posted { get; set; }

		public string PostedBy { get; set; } 

		public DateTime? PostedDate { get; set; }

        public  int? LinkDetID { get; set; }
        public  int? LinkDetStkID { get; set; }
        public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

        public virtual byte? ChType { get; set; }
        public virtual string ChNumber { get; set; }
        public virtual int? ArClass { get; set; }
        public virtual double? ARRate { get; set; }
        public virtual string ArAccID { get; set; }
        public virtual double? ArAmount { get; set; }
        public virtual string ICTaxAuth { get; set; }
        public virtual int? ICTaxClass { get; set; }
        public virtual double? ICTaxRate { get; set; }
        public virtual string ICTaxAccID { get; set; }
        public virtual double? ICTaxAmount { get; set; }
        public virtual double? InvAmount { get; set; }
    }
}
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.APINVH.Dtos
{
    public class APINVHDto : EntityDto
    {
        public int DocNo { get; set; }

        public string VAccountID { get; set; }

        public int? SubAccID { get; set; }
        public int? PendingAmt { get; set; }
        public int? CountRec { get; set; }

        public string PartyInvNo { get; set; }

        public DateTime? PartyInvDate { get; set; }

        public double? Amount { get; set; }
        public string RecAmt { get; set; }
        public string RetAmt { get; set; }
        public string BalAmt { get; set; }
        public string AccountName { get; set; }
        public string SubaccName { get; set; }

        public double? DiscAmount { get; set; }

        public string PaymentOption { get; set; }

        public string BankID { get; set; }

        public string BAccountID { get; set; }

        public int? ConfigID { get; set; }

        public string ChequeNo { get; set; }

        public int? ChType { get; set; }

        public string CurID { get; set; }

        public double? CurRate { get; set; }

        public string TaxAuth { get; set; }

        public int? TaxClass { get; set; }

        public double? TaxRate { get; set; }

        public string TaxAccID { get; set; }

        public double? TaxAmount { get; set; }

        public DateTime? DocDate { get; set; }

        public DateTime? PostDate { get; set; }

        public string Narration { get; set; }

        public string RefNo { get; set; }

        public string PayReason { get; set; }

        public bool? Posted { get; set; }
        public bool? Approve { get; set; }

        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public int? LinkDetID { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string DocStatus { get; set; }

        public int? CprID { get; set; }

        public string CprNo { get; set; }

        public DateTime? CprDate { get; set; }

    }
}
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.Invoices.Dtos
{
    public class OEINVKNOCKHDto : EntityDto
    {
        public int DocNo { get; set; }

        public int? GLLOCID { get; set; }

        public DateTime? DocDate { get; set; }

        public DateTime? PostDate { get; set; }

        public string DebtorCtrlAc { get; set; }

        public int? CustID { get; set; }

        public double? Amount { get; set; }

        public string PaymentOption { get; set; }

        public string BankID { get; set; }

        public string BAccountID { get; set; }

        public short? ConfigID { get; set; }

        public int? InsType { get; set; }

        public string InsNo { get; set; }

        public string CurID { get; set; }

        public double? CurRate { get; set; }

        public string Narration { get; set; }

        public bool Posted { get; set; }

        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public int? LinkDetID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

    }
}
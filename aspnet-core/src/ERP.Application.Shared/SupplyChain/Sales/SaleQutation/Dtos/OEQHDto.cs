using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SaleQutation.Dtos
{
    public class OEQHDto : EntityDto
    {
        public int LocID { get; set; }

        public int DocNo { get; set; }

        public DateTime? DocDate { get; set; }

        public string MDocNo { get; set; }
        public string BasicStyle { get; set; }
        public string License { get; set; }

        public string SaleTypeDesc { get; set; }
        public string CustomerDesc { get; set; }

        public DateTime? MDocDate { get; set; }

        public string TypeID { get; set; }

        public string SalesCtrlAcc { get; set; }

        public int? CustID { get; set; }

        public string Narration { get; set; }

        public string NoteText { get; set; }

        public string PayTerms { get; set; }

        public double? NetAmount { get; set; }

        public string DelvTerms { get; set; }

        public string ValidityTerms { get; set; }

        public bool Approved { get; set; }

        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string TaxAuth1 { get; set; }

        public int? TaxClass1 { get; set; }

        public string TaxClassDesc1 { get; set; }

        public double? TaxRate1 { get; set; }

        public double? TaxAmt1 { get; set; }

        public string TaxAuth2 { get; set; }

        public string TaxClassDesc2 { get; set; }

        public int? TaxClass2 { get; set; }

        public double? TaxRate2 { get; set; }

        public double? TaxAmt2 { get; set; }

        public string TaxAuth3 { get; set; }

        public string TaxClassDesc3 { get; set; }

        public int? TaxClass3 { get; set; }

        public double? TaxRate3 { get; set; }

        public double? TaxAmt3 { get; set; }

        public string TaxAuth4 { get; set; }

        public string TaxClassDesc4 { get; set; }

        public int? TaxClass4 { get; set; }

        public double? TaxRate4 { get; set; }

        public double? TaxAmt4 { get; set; }

        public string TaxAuth5 { get; set; }

        public string TaxClassDesc5 { get; set; }

        public int? TaxClass5 { get; set; }

        public double? TaxRate5 { get; set; }

        public double? TaxAmt5 { get; set; }

    }
}
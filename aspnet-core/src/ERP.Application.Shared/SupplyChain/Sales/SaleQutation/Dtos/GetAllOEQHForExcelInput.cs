using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.SaleQutation.Dtos
{
    public class GetAllOEQHForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxLocIDFilter { get; set; }
        public int? MinLocIDFilter { get; set; }

        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }

        public DateTime? MaxDocDateFilter { get; set; }
        public DateTime? MinDocDateFilter { get; set; }

        public string MDocNoFilter { get; set; }

        public DateTime? MaxMDocDateFilter { get; set; }
        public DateTime? MinMDocDateFilter { get; set; }

        public string TypeIDFilter { get; set; }

        public string SalesCtrlAccFilter { get; set; }

        public int? MaxCustIDFilter { get; set; }
        public int? MinCustIDFilter { get; set; }

        public string NarrationFilter { get; set; }

        public string NoteTextFilter { get; set; }

        public string PayTermsFilter { get; set; }

        public double? MaxNetAmountFilter { get; set; }
        public double? MinNetAmountFilter { get; set; }

        public string DelvTermsFilter { get; set; }

        public string ValidityTermsFilter { get; set; }

        public int? ApprovedFilter { get; set; }

        public string ApprovedByFilter { get; set; }

        public DateTime? MaxApprovedDateFilter { get; set; }
        public DateTime? MinApprovedDateFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

        public string TaxAuth1Filter { get; set; }

        public int? MaxTaxClass1Filter { get; set; }
        public int? MinTaxClass1Filter { get; set; }

        public string TaxClassDesc1Filter { get; set; }

        public double? MaxTaxRate1Filter { get; set; }
        public double? MinTaxRate1Filter { get; set; }

        public double? MaxTaxAmt1Filter { get; set; }
        public double? MinTaxAmt1Filter { get; set; }

        public string TaxAuth2Filter { get; set; }

        public string TaxClassDesc2Filter { get; set; }

        public int? MaxTaxClass2Filter { get; set; }
        public int? MinTaxClass2Filter { get; set; }

        public double? MaxTaxRate2Filter { get; set; }
        public double? MinTaxRate2Filter { get; set; }

        public double? MaxTaxAmt2Filter { get; set; }
        public double? MinTaxAmt2Filter { get; set; }

        public string TaxAuth3Filter { get; set; }

        public string TaxClassDesc3Filter { get; set; }

        public int? MaxTaxClass3Filter { get; set; }
        public int? MinTaxClass3Filter { get; set; }

        public double? MaxTaxRate3Filter { get; set; }
        public double? MinTaxRate3Filter { get; set; }

        public double? MaxTaxAmt3Filter { get; set; }
        public double? MinTaxAmt3Filter { get; set; }

        public string TaxAuth4Filter { get; set; }

        public string TaxClassDesc4Filter { get; set; }

        public int? MaxTaxClass4Filter { get; set; }
        public int? MinTaxClass4Filter { get; set; }

        public double? MaxTaxRate4Filter { get; set; }
        public double? MinTaxRate4Filter { get; set; }

        public double? MaxTaxAmt4Filter { get; set; }
        public double? MinTaxAmt4Filter { get; set; }

        public string TaxAuth5Filter { get; set; }

        public string TaxClassDesc5Filter { get; set; }

        public int? MaxTaxClass5Filter { get; set; }
        public int? MinTaxClass5Filter { get; set; }

        public double? MaxTaxRate5Filter { get; set; }
        public double? MinTaxRate5Filter { get; set; }

        public double? MaxTaxAmt5Filter { get; set; }
        public double? MinTaxAmt5Filter { get; set; }

    }
}
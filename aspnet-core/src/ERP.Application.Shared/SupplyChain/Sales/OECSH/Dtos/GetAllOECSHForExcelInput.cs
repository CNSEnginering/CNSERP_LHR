using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.OECSH.Dtos
{
    public class GetAllOECSHForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxLocIDFilter { get; set; }
        public int? MinLocIDFilter { get; set; }

        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }

        public DateTime? MaxDocDateFilter { get; set; }
        public DateTime? MinDocDateFilter { get; set; }

        public string SaleDocFilter { get; set; }

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

        public string BasicStyleFilter { get; set; }

        public string LicenseFilter { get; set; }

        public double? MaxDirectCostFilter { get; set; }
        public double? MinDirectCostFilter { get; set; }

        public double? MaxCommRateFilter { get; set; }
        public double? MinCommRateFilter { get; set; }

        public double? MaxCommAmtFilter { get; set; }
        public double? MinCommAmtFilter { get; set; }

        public double? MaxOHRateFilter { get; set; }
        public double? MinOHRateFilter { get; set; }

        public double? MaxOHAmtFilter { get; set; }
        public double? MinOHAmtFilter { get; set; }

        public double? MaxTaxRateFilter { get; set; }
        public double? MinTaxRateFilter { get; set; }

        public double? MaxTaxAmtFilter { get; set; }
        public double? MinTaxAmtFilter { get; set; }

        public double? MaxTotalCostFilter { get; set; }
        public double? MinTotalCostFilter { get; set; }

        public double? MaxProfRateFilter { get; set; }
        public double? MinProfRateFilter { get; set; }

        public double? MaxProfAmtFilter { get; set; }
        public double? MinProfAmtFilter { get; set; }

        public double? MaxSalePriceFilter { get; set; }
        public double? MinSalePriceFilter { get; set; }

        public double? MaxCostPPFilter { get; set; }
        public double? MinCostPPFilter { get; set; }

        public double? MaxSalePPFilter { get; set; }
        public double? MinSalePPFilter { get; set; }

        public double? MaxSaleUSFilter { get; set; }
        public double? MinSaleUSFilter { get; set; }

    }
}
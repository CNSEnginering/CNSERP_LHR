using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.OECSD.Dtos
{
    public class GetAllOECSDForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxDetIDFilter { get; set; }
        public int? MinDetIDFilter { get; set; }

        public int? MaxLocIDFilter { get; set; }
        public int? MinLocIDFilter { get; set; }

        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }

        public int? MaxTransTypeFilter { get; set; }
        public int? MinTransTypeFilter { get; set; }

        public string ItemIDFilter { get; set; }

        public string UnitFilter { get; set; }

        public decimal? MaxConverFilter { get; set; }
        public decimal? MinConverFilter { get; set; }

        public decimal? MaxQtyFilter { get; set; }
        public decimal? MinQtyFilter { get; set; }

        public decimal? MaxRateFilter { get; set; }
        public decimal? MinRateFilter { get; set; }

        public decimal? MaxAmountFilter { get; set; }
        public decimal? MinAmountFilter { get; set; }

        public string TaxAuthFilter { get; set; }

        public int? MaxTaxClassFilter { get; set; }
        public int? MinTaxClassFilter { get; set; }

        public double? MaxTaxRateFilter { get; set; }
        public double? MinTaxRateFilter { get; set; }

        public double? MaxTaxAmtFilter { get; set; }
        public double? MinTaxAmtFilter { get; set; }

        public double? MaxNetAmountFilter { get; set; }
        public double? MinNetAmountFilter { get; set; }

        public string RemarksFilter { get; set; }

    }
}
using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.SaleEntry.Dtos
{
    public class GetAllOESALEDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDetIDFilter { get; set; }
		public int? MinDetIDFilter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public string ItemIDFilter { get; set; }

		public string UnitFilter { get; set; }

		public double? MaxConverFilter { get; set; }
		public double? MinConverFilter { get; set; }

		public double? MaxQtyFilter { get; set; }
		public double? MinQtyFilter { get; set; }

		public double? MaxRateFilter { get; set; }
		public double? MinRateFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public double? MaxDiscFilter { get; set; }
		public double? MinDiscFilter { get; set; }

		public string TaxAuthFilter { get; set; }

		public int? MaxTaxClassFilter { get; set; }
		public int? MinTaxClassFilter { get; set; }

		public double? MaxTaxRateFilter { get; set; }
		public double? MinTaxRateFilter { get; set; }

		public double? MaxTaxAmtFilter { get; set; }
		public double? MinTaxAmtFilter { get; set; }

		public string RemarksFilter { get; set; }

		public double? MaxNetAmountFilter { get; set; }
		public double? MinNetAmountFilter { get; set; }



    }
}
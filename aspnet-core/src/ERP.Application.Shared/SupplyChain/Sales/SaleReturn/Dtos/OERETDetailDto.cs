
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SaleReturn.Dtos
{
    public class OERETDetailDto : EntityDto
    {
		public int DetID { get; set; }

		public int LocID { get; set; }

		public int DocNo { get; set; }

		public string ItemID { get; set; }

		public string ItemDesc { get; set; }

		public string Unit { get; set; }

		public double? Conver { get; set; }

		public double? Qty { get; set; }

		public double? Rate { get; set; }

		public double? Amount { get; set; }

		public double? Disc { get; set; }

		public string TaxAuth { get; set; }

		public string TaxAuthDesc { get; set; }

		public int? TaxClass { get; set; }

		public string TaxClassDesc { get; set; }

		public double? TaxRate { get; set; }

		public double? TaxAmt { get; set; }

		public double? Cost { get; set; }

		public double? CostAmt { get; set; }

		public string Remarks { get; set; }

		public double? NetAmount { get; set; }



    }
}
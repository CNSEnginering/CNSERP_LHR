
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.SaleReturn.Dtos
{
    public class CreateOrEditOERETDetailDto : EntityDto<int?>
    {

		[Required]
		public int DetID { get; set; }
		
		
		[Required]
		public int LocID { get; set; }
		
		
		[Required]
		public int DocNo { get; set; }
		
		
		[Required]
		public string ItemID { get; set; }
		
		
		public string Unit { get; set; }
		
		
		public double? Conver { get; set; }
		
		
		public double? Qty { get; set; }
		
		
		public double? Rate { get; set; }
		
		
		public double? Amount { get; set; }
		
		
		public double? Disc { get; set; }
		
		
		public string TaxAuth { get; set; }
		
		
		public int? TaxClass { get; set; }
		
		
		public double? TaxRate { get; set; }
		
		
		public double? TaxAmt { get; set; }
		
		
		public double? Cost { get; set; }
		
		
		public double? CostAmt { get; set; }
		
		
		public string Remarks { get; set; }
		
		
		public double? NetAmount { get; set; }
		
		

    }
}
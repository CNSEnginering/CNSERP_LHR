
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.ReceiptReturn.Dtos
{
    public class CreateOrEditPORETDetailDto : EntityDto<int?>
    {

		[Required]
		public int DetID { get; set; }
		
		
		[Required]
		public int LocID { get; set; }
		
		
		[Required]
		public int DocNo { get; set; }
		
		
		public string ItemID { get; set; }
		
		
		public string Unit { get; set; }
		
		
		public double? Conver { get; set; }
		
		
		public double? Qty { get; set; }
		
		
		public double? Rate { get; set; }
		
		
		public double? Amount { get; set; }
		
		
		public string TaxAuth { get; set; }
		
		
		public int? TaxClass { get; set; }
		
		
		public double? TaxRate { get; set; }
		
		
		public double? TaxAmt { get; set; }
		
		
		public string Remarks { get; set; }
		
		
		public double? NetAmount { get; set; }
		
		

    }
}

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class CreateOrEditVwRetQtyDto : EntityDto<int?>
    {

		[Required]
		public int LocID { get; set; }
		
		
		[Required]
		public int DocNo { get; set; }
		
		
		public string ItemID { get; set; }
		
		
		public string Unit { get; set; }
		
		
		public double? Conver { get; set; }
		
		
		public string Remarks { get; set; }
		
		
		public double? Rate { get; set; }
		
		
		public double? Qty { get; set; }
		
		
		public double? Amount { get; set; }
		
		
		public double? qtyp { get; set; }
		
		
		[Required]
		public string descp { get; set; }
		
		

    }
}
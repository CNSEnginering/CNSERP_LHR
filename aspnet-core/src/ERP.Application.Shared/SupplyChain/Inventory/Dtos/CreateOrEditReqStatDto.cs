
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditReqStatDto : EntityDto<int?>
    {

		public int? LocID { get; set; }
		
		
		public int? DocNo { get; set; }
		
		
		public string ItemID { get; set; }
		
		
		public string Unit { get; set; }
		
		
		public double? Conver { get; set; }
		
		
		public double? Qty { get; set; }
		
		
		public string Remarks { get; set; }
		
		
		public double? QIH { get; set; }
		
		
		[Required]
		public double POQty { get; set; }
		
		

    }
}
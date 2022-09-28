
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class CreateOrEditPOSTATDto : EntityDto<int?>
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
		
		
		public string Remarks { get; set; }
		
		
		[Required]
		public double Received { get; set; }
		
		
		[Required]
		public double Returned { get; set; }
		
		
		public int? ReqNo { get; set; }
		
		
		public DateTime? DocDate { get; set; }
		
		

    }
}
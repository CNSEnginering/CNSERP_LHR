
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class CreateOrEditVwReqStatus2Dto : EntityDto<int?>
    {

		public int? Locid { get; set; }
		
		
		public int? Docno { get; set; }
		
		
		public string ItemID { get; set; }
		
		
		[Required]
		public string Descp { get; set; }
		
		
		public string Unit { get; set; }
		
		
		public double? Conver { get; set; }
		
		
		public double? ReqQty { get; set; }
		
		
		public double? QIH { get; set; }
		
		
		public double? POQty { get; set; }
		
		
		public double? Received { get; set; }
		
		
		public double? Returned { get; set; }
		
		
		public double? QtyP { get; set; }
		
		

    }
}
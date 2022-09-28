
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class CreateOrEditVwReqStatusDto : EntityDto<int?>
    {

		[Required]
		public int LocID { get; set; }
		
		
		[Required]
		public int DocNo { get; set; }
		
		
		public DateTime? DocDate { get; set; }
		
		
		public string Narration { get; set; }
		
		
		public string ItemID { get; set; }
		
		
		[Required]
		public string Descp { get; set; }
		
		
		public double? reqqty { get; set; }
		
		
		[Required]
		public double poqty { get; set; }
		
		
		[Required]
		public double recqty { get; set; }
		
		
		public DateTime? podate { get; set; }
		
		
		public DateTime? recdate { get; set; }
		
		
		public string party_name { get; set; }
		
		
		public string location { get; set; }
		
		
		public string rec_narration { get; set; }
		
		
		public string OrdNo { get; set; }
		
		
		public int? recdocno { get; set; }
		
		
		public int? podocno { get; set; }
		
		
		public int? SUBCCID { get; set; }
		
		

    }
}
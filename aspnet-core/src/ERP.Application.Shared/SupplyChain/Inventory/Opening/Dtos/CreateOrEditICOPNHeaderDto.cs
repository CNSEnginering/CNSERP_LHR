
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Opening.Dtos
{
    public class CreateOrEditICOPNHeaderDto : EntityDto<int?>
    {

		[Required]
		public int LocID { get; set; }
		
		
		[Required]
		public int DocNo { get; set; }
		
		
		public DateTime? DocDate { get; set; }
		
		
		public string Narration { get; set; }
		
		
		public decimal? TotalItems { get; set; }
		
		
		public decimal? TotalQty { get; set; }
		
		
		public decimal? TotalAmt { get; set; }

        public bool Approved { get; set; }

        public bool Posted { get; set; }
		
		
		public int? LinkDetID { get; set; }
		
		
		public string OrdNo { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		

    }
}
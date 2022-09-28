
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.WorkOrder.Dtos
{
    public class CreateOrEditICWOHeaderDto : EntityDto<int?>
    {

		[Required]
		public int LocID { get; set; }
		
		
		[Required]
		public int DocNo { get; set; }
		
		
		public DateTime? DocDate { get; set; }
		
		
		public string CCID { get; set; }
		
		
		public string Narration { get; set; }


        public string Refrence { get; set; }


        public double? TotalQty { get; set; }


		public double? TotalAmt { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public bool Approved { get; set; }
		
		
		public string ApprovedBy { get; set; }
		
		
		public DateTime? ApprovedDate { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
        public string QutationDoc { get; set; }

        public bool Posted { get; set; }


    }
}
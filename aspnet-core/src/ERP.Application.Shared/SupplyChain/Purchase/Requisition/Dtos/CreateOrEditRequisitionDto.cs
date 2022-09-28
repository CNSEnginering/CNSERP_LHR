
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.SupplyChain.Purchase.Requisition.Dtos
{
    public class CreateOrEditRequisitionDto : EntityDto<int?>
    {

		[Required]
		public int LocID { get; set; }
        public string LocName { get; set; }

        [Required]
		public int? DocNo { get; set; }
		
		
		public string DocDate { get; set; }
		
		
		public string ExpArrivalDate { get; set; }
		
		
		public string OrdNo { get; set; }
		
		
		public string CCID { get; set; }
        public string CCName { get; set; }

        public string Narration { get; set; }
        public string BasicStyle { get; set; }
        public string License { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public double? OrderQty { get; set; }


        public double? TotalQty { get; set; }
		
		
		public DateTime? ArrivalDate { get; set; }
		
		[StringLength(20)]
		public string ReqNo { get; set; }
		
		
		public string AuditUser { get; set; }
		
		
		public DateTime? AuditTime { get; set; }
		
		
		public decimal? SysDate { get; set; }
		
		
		public int? DbID { get; set; }
		
		
		public bool Completed { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public bool Hold { get; set; }

        public bool Approved { get; set; }
        public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }

        public bool Posted { get; set; }
		public List<RequisitionDetailDto> requisitionDetailDto { get; set; }
    }
}
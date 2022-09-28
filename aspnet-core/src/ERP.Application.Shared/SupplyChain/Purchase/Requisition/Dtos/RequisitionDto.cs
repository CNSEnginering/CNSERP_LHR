
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.Requisition.Dtos
{
    public class RequisitionDto : EntityDto
    {
		public int LocID { get; set; }
        public string LocName { get; set; }
        public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public DateTime? ExpArrivalDate { get; set; }

		public string OrdNo { get; set; }

		public string CCID { get; set; }

		public string Narration { get; set; }

		public double? TotalQty { get; set; }

		public DateTime? ArrivalDate { get; set; }

		public string ReqNo { get; set; }

		public string AuditUser { get; set; }

		public DateTime? AuditTime { get; set; }

		public decimal? SysDate { get; set; }

		public int? DbID { get; set; }

		public bool Completed { get; set; }

		public bool Active { get; set; }

		public bool Hold { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

        public bool Approved { get; set; }

        public bool Posted { get; set; }

    }
}
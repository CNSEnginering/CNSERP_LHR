
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.WorkOrder.Dtos
{
    public class ICWOHeaderDto : EntityDto
    {
		public int LocID { get; set; }

		public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public string CCID { get; set; }

		public string CCDesc { get; set; }
		public string QutationDoc { get; set; }

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

        public bool Posted { get; set; }



    }
}
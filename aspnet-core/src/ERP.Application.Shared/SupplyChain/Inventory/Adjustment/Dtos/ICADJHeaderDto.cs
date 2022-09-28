
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Adjustment.Dtos
{
    public class ICADJHeaderDto : EntityDto
    {
		public int DocNo { get; set; }
        public int ConDocNo { get; set; }
        public int Type { get; set; }

        public DateTime? DocDate { get; set; }

		public string Narration { get; set; }

		public int? LocID { get; set; }

		public double? TotalQty { get; set; }

		public double? TotalAmt { get; set; }

		public bool Posted { get; set; }

		public bool Approved { get; set; }

        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public int? LinkDetID { get; set; }

		public string OrdNo { get; set; }

		public bool Active { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }



    }
}
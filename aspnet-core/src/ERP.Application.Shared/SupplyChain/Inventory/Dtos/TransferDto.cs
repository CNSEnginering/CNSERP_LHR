
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class TransferDto : EntityDto
    {
		public int FromLocID { get; set; }

		public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public string Narration { get; set; }

		public int? ToLocID { get; set; }

		public decimal? TotalQty { get; set; }

		public decimal? TotalAmt { get; set; }

		public bool Posted { get; set; }
        public bool Approved { get; set; }

        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public int? LinkDetID { get; set; }

		public string OrdNo { get; set; }

		public bool HOLD { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

        public string FromLocName { get; set; }
        public string ToLocName { get; set; }
        public string CCID { get; set; }
    }
}
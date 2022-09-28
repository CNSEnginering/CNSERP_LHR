
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class PORETHeaderDto : EntityDto
    {
		public int LocID { get; set; }

		public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public string AccountID { get; set; }

        public string AccDesc { get; set; }

        public int? SubAccID { get; set; }

        public string SubAccDesc { get; set; }

        public string Narration { get; set; }

		public string IGPNo { get; set; }

		public string BillNo { get; set; }

		public DateTime? BillDate { get; set; }

		public double? BillAmt { get; set; }

		public double? TotalQty { get; set; }

		public double? TotalAmt { get; set; }

		public bool Posted { get; set; }

        public bool Approved { get; set; }

        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? LinkDetID { get; set; }

		public string OrdNo { get; set; }

		public int? RecDocNo { get; set; }

		public double? Freight { get; set; }

		public double? AddExp { get; set; }

		public string CCID { get; set; }

        public string CCDesc { get; set; }

        public double? AddDisc { get; set; }

		public double? AddLeak { get; set; }

		public double? AddFreight { get; set; }

		public bool onHold { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}
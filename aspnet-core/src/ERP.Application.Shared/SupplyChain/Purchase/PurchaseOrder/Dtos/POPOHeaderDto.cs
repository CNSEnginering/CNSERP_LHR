
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.PurchaseOrder.Dtos
{
    public class POPOHeaderDto : EntityDto
    {
		public int LocID { get; set; }

		public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public DateTime? ArrivalDate { get; set; }

		public int? ReqNo { get; set; }

		public string AccountID { get; set; }

		public string AccDesc { get; set; }

        public int? SubAccID { get; set; }

        public string SubAccDesc { get; set; }

        public double? TotalQty { get; set; }

		public double? TotalAmt { get; set; }

		public string OrdNo { get; set; }

		public string CCID { get; set; }

		public string CCDesc { get; set; }

		public string Narration { get; set; }

		public int? WHTermID { get; set; }

        public string WHTermDesc { get; set; }

        public double? WHRate { get; set; }

		public string TaxAuth { get; set; }

        public string TaxAuthDesc { get; set; }

        public int? TaxClass { get; set; }

        public string TaxClassDesc { get; set; }

        public double? TaxRate { get; set; }

		public double? TaxAmount { get; set; }
        public string Terms { get; set; }

        public bool? onHold { get; set; }

		public bool? Active { get; set; }

		public bool? Completed { get; set; }
		public bool? Approved { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}
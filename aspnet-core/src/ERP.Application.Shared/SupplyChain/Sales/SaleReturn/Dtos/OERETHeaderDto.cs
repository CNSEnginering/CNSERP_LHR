
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SaleReturn.Dtos
{
    public class OERETHeaderDto : EntityDto
    {
		public int LocID { get; set; }

		public string LocDesc { get; set; }

		public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public DateTime? PaymentDate { get; set; }

		public string TypeID { get; set; }

		public string TypeDesc { get; set; }

		public int? CustID { get; set; }

        public string CustomerName { get; set; }

        public string PriceList { get; set; }

		public string Narration { get; set; }

		public string OGP { get; set; }

		public double? TotalQty { get; set; }

		public double? Amount { get; set; }

		public double? Tax { get; set; }

		public double? AddTax { get; set; }

		public double? Disc { get; set; }

		public double? TradeDisc { get; set; }

		public double? Margin { get; set; }

		public double? Freight { get; set; }

		public string OrdNo { get; set; }

		public double? TotAmt { get; set; }

		public bool Posted { get; set; }

		public int? LinkDetID { get; set; }

		public bool Active { get; set; }

		public int SDocNo { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }
        public bool? Approved { get; set; }



    }
}
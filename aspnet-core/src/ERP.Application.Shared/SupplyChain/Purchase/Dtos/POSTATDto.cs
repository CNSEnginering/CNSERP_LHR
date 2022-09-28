
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class POSTATDto : EntityDto
    {
		public int DetID { get; set; }

		public int LocID { get; set; }

		public int DocNo { get; set; }

		public string ItemID { get; set; }

		public string Unit { get; set; }

		public double? Conver { get; set; }

		public double? Qty { get; set; }

		public double? Rate { get; set; }

		public double? Amount { get; set; }

		public string Remarks { get; set; }

		public double Received { get; set; }

		public double Returned { get; set; }

		public long? ReqNo { get; set; }

		public DateTime? DocDate { get; set; }



    }
}
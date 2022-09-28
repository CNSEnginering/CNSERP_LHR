
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Consumption.Dtos
{
    public class ICCNSDetailDto : EntityDto
    {
		public int DetID { get; set; }

		public int DocNo { get; set; }

		public string ItemID { get; set; }

		public string ItemDesc { get; set; }

		public string Unit { get; set; }

		public double? Conver { get; set; }

		public double? Qty { get; set; }

		public double? QtyInHand { get; set; }

		public double? Cost { get; set; }

		public double? Amount { get; set; }

		public string Remarks { get; set; }

		public string EngNo { get; set; }

		public int? SubCCID { get; set; }

        public string SubCCName { get; set; }


    }
}
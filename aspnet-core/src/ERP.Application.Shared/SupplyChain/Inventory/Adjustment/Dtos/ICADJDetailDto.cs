
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Adjustment.Dtos
{
    public class ICADJDetailDto : EntityDto
    {
		public int DetID { get; set; }

        public int DocNo { get; set; }

		public string ItemID { get; set; }
        
		public string ItemDesc { get; set; }

		public string Unit { get; set; }

		public double? Conver { get; set; }

        public string Type { get; set; }

        public double? Qty { get; set; }

		public double? Cost { get; set; }

		public double? Amount { get; set; }

		public string Remarks { get; set; }



    }
}
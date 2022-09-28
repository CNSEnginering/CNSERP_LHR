
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class VwRetQtyDto : EntityDto
    {
		public int LocID { get; set; }

		public int DocNo { get; set; }

		public string ItemID { get; set; }

		public string Unit { get; set; }

		public double? Conver { get; set; }

		public string Remarks { get; set; }

		public double? Rate { get; set; }

		public double? Qty { get; set; }

		public double? Amount { get; set; }

		public double? qtyp { get; set; }

		public string descp { get; set; }



    }
}
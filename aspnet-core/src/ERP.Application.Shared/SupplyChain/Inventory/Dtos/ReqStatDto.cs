
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class ReqStatDto : EntityDto
    {
		public int? LocID { get; set; }

		public int? DocNo { get; set; }

		public string ItemID { get; set; }
        
		public string ItemDesc { get; set; }

		public string Unit { get; set; }

		public double? Conver { get; set; }

		public double? Qty { get; set; }

		public double? QtyP { get; set; }

		public string Remarks { get; set; }

		public double? QIH { get; set; }

		public double POQty { get; set; }



    }
}

using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Opening.Dtos
{
    public class ICOPNDetailDto : EntityDto
    {
		public int? DetID { get; set; }

		public int LocID { get; set; }

		public int DocNo { get; set; }

		public string ItemID { get; set; }

		public string ItemDesc { get; set; }

		public string Unit { get; set; }

		public decimal? Conver { get; set; }

		public decimal? Qty { get; set; }

		public decimal? Rate { get; set; }

		public decimal? Amount { get; set; }

		public string Remarks { get; set; }

		public bool Active { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }



    }
}
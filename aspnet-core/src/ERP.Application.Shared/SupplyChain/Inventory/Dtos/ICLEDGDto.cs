
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class ICLEDGDto : EntityDto
    {
		public DateTime? DocDate { get; set; }

		public int DocType { get; set; }

		public string DocDesc { get; set; }

		public int DocNo { get; set; }

		public int? LocID { get; set; }

		public string ItemID { get; set; }

		public string srno { get; set; }

		public string UNIT { get; set; }

		public double? Conver { get; set; }

		public double? Qty { get; set; }

		public double? Rate { get; set; }

		public double? Amount { get; set; }

		public string Descp { get; set; }

		public string TableName { get; set; }

		public int? PKID { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

		public string JobNo { get; set; }



    }
}
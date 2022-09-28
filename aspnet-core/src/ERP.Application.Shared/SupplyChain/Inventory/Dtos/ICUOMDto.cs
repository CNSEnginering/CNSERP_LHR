
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class ICUOMDto : EntityDto<int?>
    {
		public string Unit { get; set; }

		public string UNITDESC { get; set; }

		public double Conver { get; set; }

		public bool Active { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }



    }
}
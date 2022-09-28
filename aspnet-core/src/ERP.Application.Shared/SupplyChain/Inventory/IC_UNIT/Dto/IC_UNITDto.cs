
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.IC_UNIT.Dto
{
    public class IC_UNITDto : EntityDto
    {
		public string Unit { get; set; }

		public double Conver { get; set; }

		public short? Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

        [Required]
        public string ItemId { get; set; }



    }
}
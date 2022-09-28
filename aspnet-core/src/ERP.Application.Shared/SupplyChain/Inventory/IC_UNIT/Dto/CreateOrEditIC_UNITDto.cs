
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.IC_UNIT.Dto
{
    public class CreateOrEditIC_UNITDto : EntityDto<int?>
    {

		[Required]
		public string Unit { get; set; }
		
		
		[Required]
		public double Conver { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		
		[Required]
		public string ItemId { get; set; }



		
		

    }
}
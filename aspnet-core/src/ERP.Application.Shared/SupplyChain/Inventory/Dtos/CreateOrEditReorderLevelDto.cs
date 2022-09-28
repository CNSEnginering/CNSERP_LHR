
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditReorderLevelDto : EntityDto<int?>
    {

		public decimal? MinLevel { get; set; }
		
		
		public decimal? MaxLevel { get; set; }
		
		
		public decimal? OrdLevel { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public int LocId { get; set; }
		
		
		[Required]
		public string ItemId { get; set; }
		
		

    }
}
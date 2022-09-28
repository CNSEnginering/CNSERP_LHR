
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditICLocationDto : EntityDto<int?>
    {

		[Required]
		public int LocID { get; set; }
		
		
		public string LocName { get; set; }
		
		
		public string LocShort { get; set; }
		
		
		public string Address { get; set; }
		
		
		public string City { get; set; }
		
		
		public bool AllowRec { get; set; }
		
		
		public bool AllowNeg { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		

    }
}
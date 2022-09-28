
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.ICOPT4.Dtos
{
    public class CreateOrEditICOPT4Dto : EntityDto<int?>
    {

		[Required]
		public int OptID { get; set; }
		
		
		[Required]
		public string Descp { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
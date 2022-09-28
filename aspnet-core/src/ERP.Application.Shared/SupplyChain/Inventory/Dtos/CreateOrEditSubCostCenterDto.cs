
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditSubCostCenterDto : EntityDto<int?>
    {

		[Required]
		public string CCID { get; set; }

        public string CCName { get; set; }
        [Required]
		public int SUBCCID { get; set; }

        public string subCCName { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }

        public bool? Active { get; set; }


    }
}
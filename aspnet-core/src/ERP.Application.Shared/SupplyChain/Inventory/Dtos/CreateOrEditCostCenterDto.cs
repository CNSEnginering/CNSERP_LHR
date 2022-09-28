
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditCostCenterDto : EntityDto<int?>
    {

		[Required]
		public string CCID { get; set; }
		
		
		public string CCName { get; set; }
		
		
		public string AccountID { get; set; }

        public string AccountName { get; set; }
        public int? SubAccID { get; set; }

        public string SubAccName { get; set; }
        public Boolean Active { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
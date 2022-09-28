
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditControlDetailDto : EntityDto<int?>
    {

        public string Seg1ID { get; set; }

        [Required]
		public string SegmentName { get; set; }
		
		
		[Required]
		public int Family { get; set; }
		
		
		public string OldCode { get; set; }

        public bool Flag { get; set; }
	
		 
		 
    }
}
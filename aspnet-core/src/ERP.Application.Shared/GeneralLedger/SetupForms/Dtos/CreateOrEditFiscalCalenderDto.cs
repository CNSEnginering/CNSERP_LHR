
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditFiscalCalenderDto : EntityDto<int?>
    {

		[Required]
		public int Period { get; set; }
		
		
		[Required]
		public DateTime StartDate { get; set; }
		
		
		[Required]
		public DateTime EndDate { get; set; }
		
		
		[Required]
		public bool GL { get; set; }
		
		
		[Required]
		public bool AP { get; set; }
		
		
		[Required]
		public bool AR { get; set; }
		
		
		[Required]
		public bool IN { get; set; }
		
		
		[Required]
		public bool PO { get; set; }
		
		
		[Required]
		public bool OE { get; set; }
		
		
		[Required]
		public bool BK { get; set; }
		
		
		[Required]
		public bool HR { get; set; }
		
		
		[Required]
		public bool PR { get; set; }
		
		
		public int? CreatedBy { get; set; }
		
		
		public DateTime? CreatedDate { get; set; }
		
		
		public DateTime? EditDate { get; set; }
		
		
		public int? EditUser { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsLocked { get; set; }



    }
}
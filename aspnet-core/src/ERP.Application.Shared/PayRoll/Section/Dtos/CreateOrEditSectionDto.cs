
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Section.Dtos
{
    public class CreateOrEditSectionDto : EntityDto<int?>
    {

		[Required]
		public int SecID { get; set; }
        [Required]
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public int SortId { get; set; }

        [Required]
        public string SecName { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
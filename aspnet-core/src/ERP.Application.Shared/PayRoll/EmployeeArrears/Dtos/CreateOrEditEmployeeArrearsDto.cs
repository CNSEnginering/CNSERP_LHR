
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeArrears.Dtos
{
    public class CreateOrEditEmployeeArrearsDto : EntityDto<int?>
    {

		[Required]
		public int ArrearID { get; set; }
		
		
		[Required]
		public int EmployeeID { get; set; }

        [Required]
        public string EmployeeName { get; set; }


        [Required]
		public short SalaryYear { get; set; }
		
		
		[Required]
		public short SalaryMonth { get; set; }
		
		
		public DateTime? ArrearDate { get; set; }
		
		
		public double? Amount { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
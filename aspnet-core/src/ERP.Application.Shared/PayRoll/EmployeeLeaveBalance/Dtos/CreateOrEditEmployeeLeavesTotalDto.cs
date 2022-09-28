
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Payroll.EmployeeLeaveBalance.Dtos
{
    public class CreateOrEditEmployeeLeavesTotalDto : EntityDto<int?>
    {

		[Required]
		public int SalaryYear { get; set; }
		
		
		[Required]
		public int EmployeeID { get; set; }
		
		
		public double? Leaves { get; set; }
		
		
		public double? Casual { get; set; }
		
		
		public double? Sick { get; set; }
		
		
		public double? Annual { get; set; }
		
		
		public double? CPL { get; set; }
		
		
		[StringLength(EmployeeLeavesTotalConsts.MaxAudtUserLength, MinimumLength = EmployeeLeavesTotalConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(EmployeeLeavesTotalConsts.MaxCreatedByLength, MinimumLength = EmployeeLeavesTotalConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }

        public string EmployeeName { get; set; }

    }
}
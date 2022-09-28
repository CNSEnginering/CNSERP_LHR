
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.StopSalary.Dtos
{
    public class CreateOrEditStopSalaryDto : EntityDto<int?>
    {

        public int? TypeID { get; set; }
        public int? LoanID { get; set; }


        [Required]
        public int EmployeeID { get; set; }


        [Required]
        public short SalaryYear { get; set; }


        [Required]
        public short SalaryMonth { get; set; }


        [StringLength(StopSalaryConsts.MaxRemarksLength, MinimumLength = StopSalaryConsts.MinRemarksLength)]
        public string Remarks { get; set; }


        public bool Active { get; set; }


        [StringLength(StopSalaryConsts.MaxAudtUserLength, MinimumLength = StopSalaryConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }


        public DateTime? AudtDate { get; set; }


        [StringLength(StopSalaryConsts.MaxCreatedByLength, MinimumLength = StopSalaryConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }


        public DateTime? CreateDate { get; set; }
        public double? Amount { get; set; }

        public string EmployeeName { get; set; }


    }
}
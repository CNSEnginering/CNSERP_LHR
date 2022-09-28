using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.MonthlyCPR.Dtos
{
    public class CreateOrEditMonthlyCPRDto : EntityDto<int?>
    {

        [Required]
        public short SalaryYear { get; set; }

        [Required]
        public short SalaryMonth { get; set; }

        [StringLength(MonthlyCPRConsts.MaxCPRNumberLength, MinimumLength = MonthlyCPRConsts.MinCPRNumberLength)]
        public string CPRNumber { get; set; }

        public DateTime? CPRDate { get; set; }

        public double? Amount { get; set; }

        [StringLength(MonthlyCPRConsts.MaxRemarksLength, MinimumLength = MonthlyCPRConsts.MinRemarksLength)]
        public string Remarks { get; set; }

        public bool Active { get; set; }

        [StringLength(MonthlyCPRConsts.MaxAudtUserLength, MinimumLength = MonthlyCPRConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(MonthlyCPRConsts.MaxCreatedByLength, MinimumLength = MonthlyCPRConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
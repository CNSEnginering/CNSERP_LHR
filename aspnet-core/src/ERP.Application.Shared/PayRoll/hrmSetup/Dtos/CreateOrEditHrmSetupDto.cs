using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.hrmSetup.Dtos
{
    public class CreateOrEditHrmSetupDto : EntityDto<int?>
    {

        [StringLength(HrmSetupConsts.MaxAdvToStSalLength, MinimumLength = HrmSetupConsts.MinAdvToStSalLength)]
        public string AdvToStSal { get; set; }

        [StringLength(HrmSetupConsts.MaxAdvToPayableLength, MinimumLength = HrmSetupConsts.MinAdvToPayableLength)]
        public string AdvToPayable { get; set; }

        [StringLength(HrmSetupConsts.MaxLoanToStSalLength, MinimumLength = HrmSetupConsts.MinLoanToStSalLength)]
        public string LoanToStSal { get; set; }

        [StringLength(HrmSetupConsts.MaxLoanToPayableLength, MinimumLength = HrmSetupConsts.MinLoanToPayableLength)]
        public string LoanToPayable { get; set; }

        [StringLength(HrmSetupConsts.MaxCreatedByLength, MinimumLength = HrmSetupConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? AuditDate { get; set; }

        [StringLength(HrmSetupConsts.MaxAuditByLength, MinimumLength = HrmSetupConsts.MinAuditByLength)]
        public string AuditBy { get; set; }

    }
}
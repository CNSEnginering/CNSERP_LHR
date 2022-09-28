using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class CreateOrEditMFACSETDto : EntityDto<int?>
    {

        public int? TenantId { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxACCTSETLength, MinimumLength = MFACSETConsts.MinACCTSETLength)]
        public string ACCTSET { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxDESCLength, MinimumLength = MFACSETConsts.MinDESCLength)]
        public string DESC { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxWIPACCTLength, MinimumLength = MFACSETConsts.MinWIPACCTLength)]
        public string WIPACCT { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxSETLABACCTLength, MinimumLength = MFACSETConsts.MinSETLABACCTLength)]
        public string SETLABACCT { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxRUNLABACCTLength, MinimumLength = MFACSETConsts.MinRUNLABACCTLength)]
        public string RUNLABACCT { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxOVHACCTLength, MinimumLength = MFACSETConsts.MinOVHACCTLength)]
        public string OVHACCT { get; set; }

        [StringLength(MFACSETConsts.MaxAudtUserLength, MinimumLength = MFACSETConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(MFACSETConsts.MaxCreatedByLength, MinimumLength = MFACSETConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
        public string wipaccDesc { get; set; }
        public string labaccDesc { get; set; }
        public string runLabAccDesc { get; set; }
        public string ovhacctDesc { get; set; }

    }
}
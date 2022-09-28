using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Payroll.SlabSetup.Dtos
{
    public class CreateOrEditSlabSetupDto : EntityDto<int?>
    {

        public int? TypeID { get; set; }

        public double? SlabFrom { get; set; }

        public double? SlabTo { get; set; }

        public double? Rate { get; set; }

        public double? Amount { get; set; }

        public bool Active { get; set; }

        [StringLength(SlabSetupConsts.MaxAudtUserLength, MinimumLength = SlabSetupConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(SlabSetupConsts.MaxCreatedByLength, MinimumLength = SlabSetupConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(SlabSetupConsts.MaxModifiedByLength, MinimumLength = SlabSetupConsts.MinModifiedByLength)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifyDate { get; set; }

    }
}
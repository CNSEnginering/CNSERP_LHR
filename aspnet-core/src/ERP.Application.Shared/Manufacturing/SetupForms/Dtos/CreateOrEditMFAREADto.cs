using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class CreateOrEditMFAREADto : EntityDto<int?>
    {

        [Required]
        [StringLength(MFAREAConsts.MaxAREAIDLength, MinimumLength = MFAREAConsts.MinAREAIDLength)]
        public string AREAID { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxAREADESCLength, MinimumLength = MFAREAConsts.MinAREADESCLength)]
        public string AREADESC { get; set; }

        [Required]
        public short AREATY { get; set; }

        [Required]
        public short STATUS { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxADDRESSLength, MinimumLength = MFAREAConsts.MinADDRESSLength)]
        public string ADDRESS { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxCONTNAMELength, MinimumLength = MFAREAConsts.MinCONTNAMELength)]
        public string CONTNAME { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxCONTPOSLength, MinimumLength = MFAREAConsts.MinCONTPOSLength)]
        public string CONTPOS { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxCONTCELLLength, MinimumLength = MFAREAConsts.MinCONTCELLLength)]
        public string CONTCELL { get; set; }

        [StringLength(MFAREAConsts.MaxCONTEMAILLength, MinimumLength = MFAREAConsts.MinCONTEMAILLength)]
        public string CONTEMAIL { get; set; }

        [Required]
        public int LOCID { get; set; }
        public string LocDesc { get; set; }

        [StringLength(MFAREAConsts.MaxAudtUserLength, MinimumLength = MFAREAConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(MFAREAConsts.MaxCreatedByLength, MinimumLength = MFAREAConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
        public bool? Active { get; set; }
    }
}
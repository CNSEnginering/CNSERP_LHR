using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Manufacturing.SetupForms
{
    [Table("MFWCRES")]
    public class MFWCRES : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual int? DetID { get; set; }

        [Required]
        [StringLength(MFWCRESConsts.MaxWCIDLength, MinimumLength = MFWCRESConsts.MinWCIDLength)]
        public virtual string WCID { get; set; }

        [Required]
        [StringLength(MFWCRESConsts.MaxRESIDLength, MinimumLength = MFWCRESConsts.MinRESIDLength)]
        public virtual string RESID { get; set; }

        [Required]
        [StringLength(MFWCRESConsts.MaxRESDESCLength, MinimumLength = MFWCRESConsts.MinRESDESCLength)]
        public virtual string RESDESC { get; set; }

        [StringLength(MFWCRESConsts.MaxUOMLength, MinimumLength = MFWCRESConsts.MinUOMLength)]
        public virtual string UOM { get; set; }

        public virtual double? REQQTY { get; set; }

        public virtual double? UNITCOST { get; set; }

        public virtual double? TOTALCOST { get; set; }

    }
}
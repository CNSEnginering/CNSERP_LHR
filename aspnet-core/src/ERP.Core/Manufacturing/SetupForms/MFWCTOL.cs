using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Manufacturing.SetupForms
{
    [Table("MFWCTOL")]
    public class MFWCTOL : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual int? DetID { get; set; }

        [Required]
        [StringLength(MFWCTOLConsts.MaxWCIDLength, MinimumLength = MFWCTOLConsts.MinWCIDLength)]
        public virtual string WCID { get; set; }

        [Required]
        [StringLength(MFWCTOLConsts.MaxTOOLTYIDLength, MinimumLength = MFWCTOLConsts.MinTOOLTYIDLength)]
        public virtual string TOOLTYID { get; set; }

        [Required]
        [StringLength(MFWCTOLConsts.MaxTOOLTYDESCLength, MinimumLength = MFWCTOLConsts.MinTOOLTYDESCLength)]
        public virtual string TOOLTYDESC { get; set; }

        [StringLength(MFWCTOLConsts.MaxUOMLength, MinimumLength = MFWCTOLConsts.MinUOMLength)]
        public virtual string UOM { get; set; }

        public virtual double? REQQTY { get; set; }

        public virtual double? UNITCOST { get; set; }

        public virtual double? TOTALCOST { get; set; }

    }
}
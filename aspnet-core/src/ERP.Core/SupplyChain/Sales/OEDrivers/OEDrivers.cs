using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.OEDrivers
{
    [Table("OEDrivers")]
    public class OEDrivers : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int DriverID { get; set; }

        //[StringLength(OEDriversConsts.MaxDriverNameLength, MinimumLength = OEDriversConsts.MinDriverNameLength)]
        public virtual string DriverName { get; set; }

        public virtual bool Active { get; set; }

        //[StringLength(OEDriversConsts.MaxDriverCtrlAccLength, MinimumLength = OEDriversConsts.MinDriverCtrlAccLength)]
        public virtual string DriverCtrlAcc { get; set; }

        public virtual int? DriverSubAccID { get; set; }

        //[StringLength(OEDriversConsts.MaxCreatedByLength, MinimumLength = OEDriversConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        //[StringLength(OEDriversConsts.MaxAudtUserLength, MinimumLength = OEDriversConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

    }
}
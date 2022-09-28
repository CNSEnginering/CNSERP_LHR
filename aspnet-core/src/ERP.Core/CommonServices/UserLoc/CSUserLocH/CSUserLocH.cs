using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices.UserLoc.CSUserLocH
{
    [Table("CSUserLocH")]
    public class CSUserLocH : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual string UserId { get; set; }
        public virtual short? TypeID { get; set; }

        [StringLength(CSUserLocHConsts.MaxCreatedByLength, MinimumLength = CSUserLocHConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        [StringLength(CSUserLocHConsts.MaxAudtUserLength, MinimumLength = CSUserLocHConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

    }
}
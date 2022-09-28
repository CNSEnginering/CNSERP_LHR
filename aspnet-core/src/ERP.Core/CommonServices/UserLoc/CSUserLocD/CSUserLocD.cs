using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices.UserLoc.CSUserLocD
{
    [Table("CSUserLocD")]
    public class CSUserLocD : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual short? TypeID { get; set; }

        [StringLength(CSUserLocDConsts.MaxUserIDLength, MinimumLength = CSUserLocDConsts.MinUserIDLength)]
        public virtual string UserID { get; set; }

        public int? DetId { get; set; }
        public int? LocId { get; set; }

        public virtual bool Status { get; set; }

    }
}
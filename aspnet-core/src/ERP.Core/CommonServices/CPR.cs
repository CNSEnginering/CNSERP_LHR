using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.CommonServices
{
        [Table("CSCPR")]
        public class CPR : Entity, IMustHaveTenant
        {
            public int TenantId { get; set; }

            [Required]
            public virtual int CprId { get; set; }

            [Required]
            public virtual string CprNo { get; set; }

            public virtual bool Active { get; set; }

            public virtual string AudtUser { get; set; }

            public virtual DateTime? AudtDate { get; set; }

            public virtual string CreatedBy { get; set; }

            public virtual DateTime? CreateDate { get; set; }

        }

}

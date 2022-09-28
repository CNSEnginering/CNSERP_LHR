using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace ERP.SupplyChain.Inventory
{
    [Table("ICGPH")]
    public class GatePass : Entity, IMustHaveTenant
    {
        public GatePass()
        {
            // GatePasses = new HashSet<GatePassDetail>();
        }
        public int TenantId { get; set; }
        [Required]
        public virtual short TypeID { get; set; }
        [Required]
        public virtual int DocNo { get; set; }
        public string OrderNo { get; set; }
        public virtual DateTime? DocDate { get; set; }
        public virtual string AccountID { get; set; }
        public virtual int? PartyID { get; set; }
        public virtual string Narration { get; set; }
        public virtual short? GPType { get; set; }
        public virtual string DriverName { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual int? GPDocNo { get; set; }
        public virtual string AudtUser { get; set; }
        public virtual DateTime? AudtDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual bool Approved { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }
        //public virtual ICollection<GatePassDetail> GatePasses { get; set; }
        public virtual string DCNo { get; set; }
    }
}
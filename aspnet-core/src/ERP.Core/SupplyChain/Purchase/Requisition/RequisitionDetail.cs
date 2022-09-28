using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.SupplyChain.Purchase.Requisition
{
    [Table("POREQD")]
    public class RequisitionDetail : Entity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual int DetID { get; set; }
        public virtual int LocID { get; set; }
        public virtual int? DocNo { get; set; }
        [StringLength(14)]
        public virtual string ItemID { get; set; }
        public string TransName { get; set; }
        public virtual int? TransId { get; set; }
       
        [StringLength(7)]
        public virtual string Unit { get; set; }
        public virtual double Conver { get; set; }
        public virtual double Qty { get; set; }
        [StringLength(50)]
        public virtual string Remarks { get; set; }
        public virtual double QIH { get; set; }
        public virtual int SUBCCID { get; set; }
        public virtual double? QtyInPo { get; set; }
    }
}

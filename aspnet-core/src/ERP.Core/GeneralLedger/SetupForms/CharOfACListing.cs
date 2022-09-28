using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("vwGLAMF")]
    public class CharOfACListing : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual string AccountID { get; set; }
        public virtual string AccountName { get; set; }
        public virtual string GRPCTDESC { get; set; }
        public virtual bool SubLedger { get; set; }
        public virtual string GRPDESC { get; set; }
        public virtual short? SLType { get; set; }
        public virtual string Seg1ID { get; set; }
        public virtual string Seg2ID { get; set; }
        public virtual string Seg3ID { get; set; }
        public virtual string Seg1Name { get; set; }
        public virtual string Seg2Name { get; set; }
        public virtual string Seg3Name { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("GLSEG3")]
    public class Segmentlevel3 : Entity , IMustHaveTenant
    {
		public int TenantId { get; set; }

      
        public virtual string Seg3ID { get; set; }
        public virtual string SegmentName { get; set; }
		
		public virtual string OldCode { get; set; }
		
    }
}
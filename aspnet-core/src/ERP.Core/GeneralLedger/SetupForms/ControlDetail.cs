using ERP.GeneralLedger.SetupForms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("GLSEG1")]
    public class ControlDetail : Entity , IMustHaveTenant
    {
	   			
        public int TenantId { get; set; }

        [Required]
        public virtual string Seg1ID { get; set; }

        [Required]
		public virtual string SegmentName { get; set; }
	
		public virtual string OldCode { get; set; }

        public virtual int Family { get; set; }

    }
}
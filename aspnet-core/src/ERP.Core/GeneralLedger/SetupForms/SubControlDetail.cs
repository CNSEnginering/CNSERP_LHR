using ERP.GeneralLedger.SetupForms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("GLSEG2")]
    public class SubControlDetail : Entity , IMustHaveTenant
    {
        
        public int TenantId { get; set; }
        public virtual string Seg1ID { get; set; }

        public virtual string Seg2ID { get; set; }

        public virtual string SegmentName { get; set; }
		
		public virtual string OldCode { get; set; }
        public virtual string AccountType { get; set; }

        public virtual int? AccountHeader { get; set; }

        public virtual int? SortOrder { get; set; }

        public virtual string AccountBSType { get; set; }

        public virtual int? AccountBSHeader { get; set; }

        public virtual int? SortBSOrder { get; set; }
        public virtual string AccountCFType { get; set; }

        public virtual int? AccountCFHeader { get; set; }

        public virtual int? SortCFOrder { get; set; }

    }
}
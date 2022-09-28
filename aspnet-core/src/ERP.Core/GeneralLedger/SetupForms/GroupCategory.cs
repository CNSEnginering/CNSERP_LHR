using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("GLGRPCAT")]
    public class GroupCategory : Entity , IMustHaveTenant
    {
        public virtual int GRPCTCODE { get; set; }
        public int TenantId { get; set; }
		public virtual string GRPCTDESC { get; set; }
    }
}
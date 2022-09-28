using ERP.GeneralLedger.SetupForms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("GLGRPCODE")]
    public class GroupCode : Entity , IMustHaveTenant
    {
	    public int TenantId { get; set; }
     
        public virtual int GRPCODE { get; set; }

        public virtual string GRPDESC { get; set; }
		
		public virtual int GRPCTCODE { get; set; }
		
    }
}
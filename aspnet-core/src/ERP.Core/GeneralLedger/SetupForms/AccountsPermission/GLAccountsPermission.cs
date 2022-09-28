using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms.AccountsPermission
{
	[Table("GLSEC")]
    public class GLAccountsPermission : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string UserID { get; set; }
		
		public virtual long? CanSee { get; set; }
		
		public virtual string BeginAcc { get; set; }
		
		public virtual string EndAcc { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		

    }
}
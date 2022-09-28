using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.SaleAccounts
{
	[Table("OECOLL")]
    public class OECOLL : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int LocID { get; set; }
		
		[Required]
		public virtual string TypeID { get; set; }
		
		public virtual string SalesACC { get; set; }
		
		public virtual string SalesRetACC { get; set; }
		
		public virtual string COGSACC { get; set; }
		
		public virtual string ChAccountID { get; set; }
		
		public virtual string DiscAcc { get; set; }
		
		public virtual string WriteOffAcc { get; set; }
        public virtual string PayableAcc { get; set; }
        public virtual string RefundableAcc { get; set; }

        public virtual short? Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}
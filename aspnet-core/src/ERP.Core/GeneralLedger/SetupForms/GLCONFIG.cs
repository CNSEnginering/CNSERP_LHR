using ERP.GeneralLedger.SetupForms;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("GLCONFIG")]
    public class GLCONFIG : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string AccountID { get; set; }
		
		[Required]
		public virtual int SubAccID { get; set; }
		
		[Required]
		public virtual int ConfigID { get; set; }
		
		[Required]
		public virtual string BookID { get; set; }
		
		public virtual bool PostingOn { get; set; }
		
		public virtual DateTime? AUDTDATE { get; set; }
		
		public virtual string AUDTUSER { get; set; }

        public virtual string BANKID { get; set; }


    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("CSFISCALLOCK")]
    public class FiscalCalender : Entity , IMayHaveTenant
    {
	    public int? TenantId { get; set; }			

		[Required]
		public virtual int Period { get; set; }
		
		[Required]
		public virtual DateTime StartDate { get; set; }
		
		[Required]
		public virtual DateTime EndDate { get; set; }
		
		[Required]
		public virtual bool GL { get; set; }
		
		[Required]
		public virtual bool AP { get; set; }
		
		[Required]
		public virtual bool AR { get; set; }
		
		[Required]
		public virtual bool IN { get; set; }
		
		[Required]
		public virtual bool PO { get; set; }
		
		[Required]
		public virtual bool OE { get; set; }
		
		[Required]
		public virtual bool BK { get; set; }
		
		[Required]
		public virtual bool HR { get; set; }
		
		[Required]
		public virtual bool PR { get; set; }
		
		public virtual int? CreatedBy { get; set; }
		
		public virtual DateTime? CreatedDate { get; set; }
		
		public virtual DateTime? EditDate { get; set; }
		
		public virtual int? EditUser { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual bool IsLocked { get; set; }


    }
}
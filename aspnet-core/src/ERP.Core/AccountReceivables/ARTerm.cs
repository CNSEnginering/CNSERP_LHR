using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.AccountReceivables
{
	[Table("ARTerms")]
    public class ARTerm : Entity , IMustHaveTenant
    {
	    public int TenantId { get; set; }

        [Required]
		public virtual int TermID { get; set; }
		
		public virtual string TERMDESC { get; set; }
		
		public virtual double? TERMRATE { get; set; }
		
		[StringLength(ARTermConsts.MaxTERMACCIDLength, MinimumLength = ARTermConsts.MinTERMACCIDLength)]
		public virtual string TERMACCID { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(ARTermConsts.MaxAudtUserLength, MinimumLength = ARTermConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(ARTermConsts.MaxCreatedByLength, MinimumLength = ARTermConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}
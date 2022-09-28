using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.DeductionTypes
{
	[Table("DeductionTypes")]
    public class DeductionTypes : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int TypeID { get; set; }
		
		[StringLength(DeductionTypesConsts.MaxTypeDescLength, MinimumLength = DeductionTypesConsts.MinTypeDescLength)]
		public virtual string TypeDesc { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(DeductionTypesConsts.MaxAudtUserLength, MinimumLength = DeductionTypesConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(DeductionTypesConsts.MaxCreatedByLength, MinimumLength = DeductionTypesConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}
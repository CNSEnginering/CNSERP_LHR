using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.EarningTypes
{
	[Table("EarningTypes")]
    public class EarningTypes : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int TypeID { get; set; }
		
		[StringLength(EarningTypesConsts.MaxTypeDescLength, MinimumLength = EarningTypesConsts.MinTypeDescLength)]
		public virtual string TypeDesc { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(EarningTypesConsts.MaxAudtUserLength, MinimumLength = EarningTypesConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(EarningTypesConsts.MaxCreatedByLength, MinimumLength = EarningTypesConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}
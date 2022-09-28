using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.SubDesignations
{
	[Table("SubDesignations")]
    public class SubDesignations : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int SubDesignationID { get; set; }
		
		[StringLength(SubDesignationsConsts.MaxSubDesignationLength, MinimumLength = SubDesignationsConsts.MinSubDesignationLength)]
		public virtual string SubDesignation { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(SubDesignationsConsts.MaxAudtUserLength, MinimumLength = SubDesignationsConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(SubDesignationsConsts.MaxCreatedByLength, MinimumLength = SubDesignationsConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICELocation")]
    public class ICELocation : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }

        public int? ParentId { get; set; }
                                           //[Required]
                                           //public virtual int ID { get; set; }
		
		[Required]
		[StringLength(ICELocationConsts.MaxLocationTitleLength, MinimumLength = ICELocationConsts.MinLocationTitleLength)]
		public virtual string LocationTitle { get; set; }
		
		[StringLength(ICELocationConsts.MaxAudtUserLength, MinimumLength = ICELocationConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(ICELocationConsts.MaxApprovedByLength, MinimumLength = ICELocationConsts.MinApprovedByLength)]
		public virtual string ApprovedBy { get; set; }
		
		public virtual DateTime? ApprovedDate { get; set; }
		

    }
}
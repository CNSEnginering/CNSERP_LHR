using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.AllowanceSetup
{
	[Table("AllowanceSetup")]
    public class AllowanceSetup : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? DocID { get; set; }
		
		public virtual double? FuelRate { get; set; }
        public virtual DateTime? FuelDate { get; set; }
        public virtual double? PerLiterMilage { get; set; }
		
		public virtual double? MilageRate { get; set; }
		
		public virtual double? RepairRate { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(AllowanceSetupConsts.MaxAudtUserLength, MinimumLength = AllowanceSetupConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(AllowanceSetupConsts.MaxCreatedByLength, MinimumLength = AllowanceSetupConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}
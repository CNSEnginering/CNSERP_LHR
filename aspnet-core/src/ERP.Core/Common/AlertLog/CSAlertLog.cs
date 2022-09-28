using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common.AlertLog
{
	[Table("CSAlertLog")]
    public class CSAlertLog : Entity , IMustHaveTenant
    {
	    public int TenantId { get; set; }
			
        public virtual int AlertId { get; set; }

        public virtual DateTime? SentDate { get; set; }
		
		[StringLength(CSAlertLogConsts.MaxUserHostLength, MinimumLength = CSAlertLogConsts.MinUserHostLength)]
		public virtual string UserHost { get; set; }
		
		[StringLength(CSAlertLogConsts.MaxLogInUserLength, MinimumLength = CSAlertLogConsts.MinLogInUserLength)]
		public virtual string LogInUser { get; set; }
		
		public virtual short? Active { get; set; }
		
		[StringLength(CSAlertLogConsts.MaxAudtUserLength, MinimumLength = CSAlertLogConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(CSAlertLogConsts.MaxCreatedByLength, MinimumLength = CSAlertLogConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}
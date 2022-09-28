using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Holidays
{
	[Table("Holidays")]
    public class Holidays : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int HolidayID { get; set; }
		
		[Required]
		public virtual DateTime HolidayDate { get; set; }
		
		[StringLength(HolidaysConsts.MaxHolidayNameLength, MinimumLength = HolidaysConsts.MinHolidayNameLength)]
		public virtual string HolidayName { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(HolidaysConsts.MaxAudtUserLength, MinimumLength = HolidaysConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(HolidaysConsts.MaxCreatedByLength, MinimumLength = HolidaysConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}
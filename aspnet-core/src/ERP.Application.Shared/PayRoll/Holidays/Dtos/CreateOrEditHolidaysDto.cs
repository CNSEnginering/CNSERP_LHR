
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Holidays.Dtos
{
    public class CreateOrEditHolidaysDto : EntityDto<int?>
    {

		[Required]
		public int HolidayID { get; set; }
		
		
		[Required]
		public DateTime HolidayDate { get; set; }
		
		
		[StringLength(HolidaysConsts.MaxHolidayNameLength, MinimumLength = HolidaysConsts.MinHolidayNameLength)]
		public string HolidayName { get; set; }
		
		
		public bool Active { get; set; }
		
		
		[StringLength(HolidaysConsts.MaxAudtUserLength, MinimumLength = HolidaysConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(HolidaysConsts.MaxCreatedByLength, MinimumLength = HolidaysConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}
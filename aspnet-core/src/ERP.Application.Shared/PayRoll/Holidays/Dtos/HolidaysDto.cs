
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Holidays.Dtos
{
    public class HolidaysDto : EntityDto
    {
		public int HolidayID { get; set; }

		public DateTime HolidayDate { get; set; }

		public string HolidayName { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}
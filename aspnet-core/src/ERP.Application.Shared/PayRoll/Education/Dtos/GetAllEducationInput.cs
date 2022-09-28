using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Education.Dtos
{
    public class GetAllEducationInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxEdIDFilter { get; set; }
		public int? MinEdIDFilter { get; set; }

		public string EductionFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
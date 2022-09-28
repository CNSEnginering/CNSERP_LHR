using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.AlertLog.Dtos
{
    public class GetAllCSAlertLogInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxSentDateFilter { get; set; }
		public DateTime? MinSentDateFilter { get; set; }

		public string UserHostFilter { get; set; }

		public string LogInUserFilter { get; set; }

		public short? MaxActiveFilter { get; set; }
		public short? MinActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
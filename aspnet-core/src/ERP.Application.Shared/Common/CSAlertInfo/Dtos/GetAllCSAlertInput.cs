using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.CSAlertInfo.Dtos
{
    public class GetAllCSAlertInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string AlertDescFilter { get; set; }

		public string AlertSubjectFilter { get; set; }

		public string AlertBodyFilter { get; set; }

		public string SendToEmailFilter { get; set; }

		public short? MaxActiveFilter { get; set; }
		public short? MinActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }

		public int? MaxAlertIdFilter { get; set; }
		public int? MinAlertIdFilter { get; set; }



    }
}
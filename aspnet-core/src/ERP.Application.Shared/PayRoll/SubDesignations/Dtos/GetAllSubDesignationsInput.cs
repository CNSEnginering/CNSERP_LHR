using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.SubDesignations.Dtos
{
    public class GetAllSubDesignationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxSubDesignationIDFilter { get; set; }
		public int? MinSubDesignationIDFilter { get; set; }

		public string SubDesignationFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
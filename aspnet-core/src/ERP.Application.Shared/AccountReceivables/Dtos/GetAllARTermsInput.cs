using Abp.Application.Services.Dto;
using System;

namespace ERP.AccountReceivables.Dtos
{
    public class GetAllARTermsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTermIDFilter { get; set; }
		public int? MinTermIDFilter { get; set; }

		public string TERMDESCFilter { get; set; }

		public double? MaxTERMRATEFilter { get; set; }
		public double? MinTERMRATEFilter { get; set; }

		public string TERMACCIDFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
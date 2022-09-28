using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllFiscalCalendersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxPeriodFilter { get; set; }
		public int? MinPeriodFilter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public DateTime? MaxEndDateFilter { get; set; }
		public DateTime? MinEndDateFilter { get; set; }

		public int GLFilter { get; set; }

		public int APFilter { get; set; }

		public int ARFilter { get; set; }

		public int INFilter { get; set; }

		public int POFilter { get; set; }

		public int OEFilter { get; set; }

		public int BKFilter { get; set; }

		public int HRFilter { get; set; }

		public int PRFilter { get; set; }

		public int? MaxCreatedByFilter { get; set; }
		public int? MinCreatedByFilter { get; set; }

		public DateTime? MaxCreatedDateFilter { get; set; }
		public DateTime? MinCreatedDateFilter { get; set; }

		public DateTime? MaxEditDateFilter { get; set; }
		public DateTime? MinEditDateFilter { get; set; }

		public int? MaxEditUserFilter { get; set; }
		public int? MinEditUserFilter { get; set; }



    }
}
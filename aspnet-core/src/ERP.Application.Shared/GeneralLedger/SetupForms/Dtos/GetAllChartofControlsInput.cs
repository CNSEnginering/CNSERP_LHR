using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllChartofControlsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string AccountIDFilter { get; set; }

		public string AccountNameFilter { get; set; }

		public int SubLedgerFilter { get; set; }

		public int? MaxOptFldFilter { get; set; }
		public int? MinOptFldFilter { get; set; }

		public short? MaxSLTypeFilter { get; set; }
		public short? MinSLTypeFilter { get; set; }

		public int InactiveFilter { get; set; }

		public DateTime? MaxCreationDateFilter { get; set; }
		public DateTime? MinCreationDateFilter { get; set; }

		public string AuditUserFilter { get; set; }

		public DateTime? MaxAuditTimeFilter { get; set; }
		public DateTime? MinAuditTimeFilter { get; set; }

		public string OldCodeFilter { get; set; }


		 public string ControlDetailSegmentNameFilter { get; set; }

		 		 public string SubControlDetailSegmentNameFilter { get; set; }

		 		 public string Segmentlevel3SegmentNameFilter { get; set; }

		 
    }
}
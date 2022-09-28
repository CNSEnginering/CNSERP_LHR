using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllICSetupsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string Segment1Filter { get; set; }

		public string Segment2Filter { get; set; }

		public string Segment3Filter { get; set; }

		public int? MaxErrSrNoFilter { get; set; }
		public int? MinErrSrNoFilter { get; set; }

		public int? MaxCostingMethodFilter { get; set; }
		public int? MinCostingMethodFilter { get; set; }

		public string PRBookIDFilter { get; set; }

		public string RTBookIDFilter { get; set; }

		public string CnsBookIDFilter { get; set; }

		public string SLBookIDFilter { get; set; }

		public string SRBookIDFilter { get; set; }

		public string TRBookIDFilter { get; set; }

		public string PrdBookIDFilter { get; set; }

		public string PyRecBookIDFilter { get; set; }

		public string AdjBookIDFilter { get; set; }

		public string AsmBookIDFilter { get; set; }

		public string WSBookIDFilter { get; set; }

		public string DSBookIDFilter { get; set; }

		public int? MaxCurrentLocIDFilter { get; set; }
		public int? MinCurrentLocIDFilter { get; set; }

		public string Opt4Filter { get; set; }

		public string Opt5Filter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateadOnFilter { get; set; }
		public DateTime? MinCreateadOnFilter { get; set; }



    }
}
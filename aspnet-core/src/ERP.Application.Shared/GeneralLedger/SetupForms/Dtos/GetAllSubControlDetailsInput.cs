using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllSubControlDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string Seg2IDFilter { get; set; }

		public string SegmentNameFilter { get; set; }

		public string OldCodeFilter { get; set; }


		 public string ControlDetailIdFilter { get; set; }

		 
    }
}
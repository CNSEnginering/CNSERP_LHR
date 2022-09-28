using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllControlDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

        public string Seg1IDFilter { get; set; }
        public string SegmentNameFilter { get; set; }

		public string FamilyFilter { get; set; }
		

		public string OldCodeFilter { get; set; }

		 
    }
}
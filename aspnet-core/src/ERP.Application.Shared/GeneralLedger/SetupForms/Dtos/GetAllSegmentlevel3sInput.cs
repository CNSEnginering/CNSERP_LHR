using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllSegmentlevel3sInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string Seg3IDFilter { get; set; }

        public string Seg1Filter { get; set; }

        public string Seg2Filter { get; set; }

        public string SegmentNameFilter { get; set; }

	


		
		 
    }
}
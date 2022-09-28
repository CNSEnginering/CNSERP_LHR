using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllGLCONFIGInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxSubAccIDFilter { get; set; }
		public int? MinSubAccIDFilter { get; set; }

		public int? MaxConfigIDFilter { get; set; }
		public int? MinConfigIDFilter { get; set; }

		public string BookIDFilter { get; set; }

		public int PostingOnFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }


		 public string GLBOOKSBookNameFilter { get; set; }

		 		 public string ChartofControlAccountNameFilter { get; set; }

		 		 public string AccountSubLedgerSubAccNameFilter { get; set; }

		 
    }
}
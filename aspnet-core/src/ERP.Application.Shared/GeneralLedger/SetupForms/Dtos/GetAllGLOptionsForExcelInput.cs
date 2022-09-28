using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllGLOptionsForExcelInput
    {
		public string Filter { get; set; }

		public string DEFAULTCLACCFilter { get; set; }
		public string STOCKCTRLACCFilter { get; set; }

		public string Seg1NameFilter { get; set; }

		public string Seg2NameFilter { get; set; }

		public string Seg3NameFilter { get; set; }

		public int DirectPostFilter { get; set; }
		public int AutoSeg3Filter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }


		 public string ChartofControlIdFilter { get; set; }

		 
    }
}
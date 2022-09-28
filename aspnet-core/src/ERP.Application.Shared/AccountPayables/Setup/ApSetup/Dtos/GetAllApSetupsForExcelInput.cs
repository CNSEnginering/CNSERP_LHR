using Abp.Application.Services.Dto;
using System;

namespace ERP.AccountPayables.Setup.ApSetup.Dtos
{
    public class GetAllApSetupsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxDEFBANKIDFilter { get; set; }
		public int? MinDEFBANKIDFilter { get; set; }

		public int? MaxDEFPAYCODEFilter { get; set; }
		public int? MinDEFPAYCODEFilter { get; set; }

		public string DEFVENCTRLACCFilter { get; set; }

		public int? MaxDEFCURRCODEFilter { get; set; }
		public int? MinDEFCURRCODEFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }



    }
}
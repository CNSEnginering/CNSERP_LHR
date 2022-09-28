using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllGLBOOKSForExcelInput
    {
		public string Filter { get; set; }

		public string BookIDFilter { get; set; }

		public string BookNameFilter { get; set; }

		public int? MaxNormalEntryFilter { get; set; }
		public int? MinNormalEntryFilter { get; set; }

		public int IntegratedFilter { get; set; }

		public int INACTIVEFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }

		public short? MaxRestrictedFilter { get; set; }
		public short? MinRestrictedFilter { get; set; }



    }
}
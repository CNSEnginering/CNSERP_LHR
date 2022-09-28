using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllTaxAuthoritiesForExcelInput
    {
		public string Filter { get; set; }

		public string CMPIDFilter { get; set; }

		public string TAXAUTHFilter { get; set; }

		public string TAXAUTHDESCFilter { get; set; }

		public string ACCLIABILITYFilter { get; set; }

		public string ACCRECOVERABLEFilter { get; set; }

		public double? MaxRECOVERRATEFilter { get; set; }
		public double? MinRECOVERRATEFilter { get; set; }

		public string ACCEXPENSEFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }


		 public string CompanyProfileIdFilter { get; set; }

		 
    }
}
using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.Dtos
{
    public class GetAllTaxClassesForExcelInput
    {
		public string Filter { get; set; }

		public string TAXAUTHFilter { get; set; }

		public string CLASSDESCFilter { get; set; }

		public double? MaxCLASSRATEFilter { get; set; }
		public double? MinCLASSRATEFilter { get; set; }

		public double? MaxTRANSTYPEFilter { get; set; }
		public double? MinTRANSTYPEFilter { get; set; }

		public string CLASSTYPEFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }


		 public string TaxAuthorityTAXAUTHFilter { get; set; }

		 
    }
}
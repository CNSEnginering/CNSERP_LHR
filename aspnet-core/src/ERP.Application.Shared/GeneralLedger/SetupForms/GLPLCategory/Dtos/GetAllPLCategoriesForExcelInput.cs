using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos
{
    public class GetAllPLCategoriesForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxTenantIDFilter { get; set; }
		public int? MinTenantIDFilter { get; set; }

		public string TypeIDFilter { get; set; }

		public string HeadingTextFilter { get; set; }

		public short? MaxSortOrderFilter { get; set; }
		public short? MinSortOrderFilter { get; set; }



    }
}
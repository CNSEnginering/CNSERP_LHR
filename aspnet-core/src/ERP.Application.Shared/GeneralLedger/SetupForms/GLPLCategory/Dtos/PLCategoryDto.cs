
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos
{
    public class PLCategoryDto : EntityDto
    {
		public int? TenantID { get; set; }

		public string TypeID { get; set; }

		public string HeadingText { get; set; }

		public short? SortOrder { get; set; }



    }
}
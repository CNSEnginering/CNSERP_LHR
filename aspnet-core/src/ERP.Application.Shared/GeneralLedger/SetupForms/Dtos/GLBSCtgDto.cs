
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GLBSCtgDto : EntityDto
    {
		public int? TenantID { get; set; }

		public string BSType { get; set; }

		public string BSAccType { get; set; }

		public string BSAccDesc { get; set; }

		public int? SortOrder { get; set; }

		public int? BSGID { get; set; }



    }
}
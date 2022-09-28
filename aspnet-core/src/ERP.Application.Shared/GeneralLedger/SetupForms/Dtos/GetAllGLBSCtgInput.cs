using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllGLBSCtgInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTenantIDFilter { get; set; }
		public int? MinTenantIDFilter { get; set; }

		public string BSTypeFilter { get; set; }

		public string BSAccTypeFilter { get; set; }

		public string BSAccDescFilter { get; set; }

		public int? MaxSortOrderFilter { get; set; }
		public int? MinSortOrderFilter { get; set; }

		public int? MaxBSGIDFilter { get; set; }
		public int? MinBSGIDFilter { get; set; }



    }
}
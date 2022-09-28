using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllGLLocationsForExcelInput
    {
		public string Filter { get; set; }

		public string LocDescFilter { get; set; }

		public string AuditUserFilter { get; set; }

		public DateTime? MaxAuditDateFilter { get; set; }
		public DateTime? MinAuditDateFilter { get; set; }

		public int? MaxLocIdFilter { get; set; }
		public int? MinLocIdFilter { get; set; }



    }
}
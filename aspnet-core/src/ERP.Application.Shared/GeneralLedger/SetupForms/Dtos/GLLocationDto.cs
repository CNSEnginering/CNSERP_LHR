
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GLLocationDto : EntityDto
    {
		public string LocDesc { get; set; }

		public string AuditUser { get; set; }

		public DateTime? AuditDate { get; set; }

		public int LocId { get; set; }

		public int? CityID { get; set; }

		public string PreFix { get; set; }

	}
}

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditGLLocationDto : EntityDto<int?>
    {

		public string LocDesc { get; set; }
		
		public string AuditUser { get; set; }
		
		public DateTime? AuditDate { get; set; }
		
		public int LocId { get; set; }

		public int? CityID { get; set; }

		public string PreFix { get; set; }

	}
}
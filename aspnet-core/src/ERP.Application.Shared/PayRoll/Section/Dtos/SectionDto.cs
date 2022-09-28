
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Section.Dtos
{
    public class SectionDto : EntityDto
    {
		public int SecID { get; set; }

		public string SecName { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}
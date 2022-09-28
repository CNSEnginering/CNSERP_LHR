
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Education.Dtos
{
    public class EducationDto : EntityDto
    {
		public int EdID { get; set; }

		public string Eduction { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}

using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.CSAlertInfo.Dtos
{
    public class CSAlertDto : EntityDto
    {
		public string AlertDesc { get; set; }

		public string AlertSubject { get; set; }

		public string AlertBody { get; set; }

		public string SendToEmail { get; set; }

		public short? Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

		public int AlertId { get; set; }



    }
}
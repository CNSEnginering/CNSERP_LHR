
using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.AlertLog.Dtos
{
    public class CSAlertLogDto : EntityDto
    {

        public  int AlertId { get; set; }
        public DateTime? SentDate { get; set; }

		public string UserHost { get; set; }

		public string LogInUser { get; set; }

		public short? Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }


        public string AlertMessage { get; set; }
    }
}
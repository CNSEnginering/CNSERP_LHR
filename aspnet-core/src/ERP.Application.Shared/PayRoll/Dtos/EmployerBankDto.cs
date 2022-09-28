
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Dtos
{
    public class EmployerBankDto : EntityDto
    {
		public int EBankID { get; set; }

		public string EBankName { get; set; }
        public string EBankAccNumber { get; set; }
        public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}
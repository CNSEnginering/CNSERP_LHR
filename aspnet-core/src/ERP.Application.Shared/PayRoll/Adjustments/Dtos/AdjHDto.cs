
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Adjustments.Dtos
{
    public class AdjHDto : EntityDto
    {
		public int? TenantID { get; set; }

		public int? DocType { get; set; }

		public int? TypeID { get; set; }

		public int? DocID { get; set; }

		public DateTime? Docdate { get; set; }

		public short SalaryYear { get; set; }

		public short SalaryMonth { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}
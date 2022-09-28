
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class AllowancesDto : EntityDto
    {
		public int? DocID { get; set; }

		public DateTime Docdate { get; set; }

		public short? DocMonth { get; set; }

		public int? DocYear { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}

using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.RecurringVoucher.Dtos
{
    public class RecurringVoucherDto : EntityDto
    {
		public int? DocNo { get; set; }

		public string BookID { get; set; }

		public int? VoucherNo { get; set; }

		public string FmtVoucherNo { get; set; }

		public DateTime? VoucherDate { get; set; }

		public int? VoucherMonth { get; set; }

		public int? ConfigID { get; set; }

		public string Reference { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}
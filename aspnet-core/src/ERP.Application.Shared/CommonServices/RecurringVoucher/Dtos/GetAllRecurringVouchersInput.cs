using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.RecurringVoucher.Dtos
{
    public class GetAllRecurringVouchersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public string BookIDFilter { get; set; }

		public int? MaxVoucherNoFilter { get; set; }
		public int? MinVoucherNoFilter { get; set; }

		public string FmtVoucherNoFilter { get; set; }

		public DateTime? MaxVoucherDateFilter { get; set; }
		public DateTime? MinVoucherDateFilter { get; set; }

		public int? MaxVoucherMonthFilter { get; set; }
		public int? MinVoucherMonthFilter { get; set; }

		public int? MaxConfigIDFilter { get; set; }
		public int? MinConfigIDFilter { get; set; }

		public string ReferenceFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
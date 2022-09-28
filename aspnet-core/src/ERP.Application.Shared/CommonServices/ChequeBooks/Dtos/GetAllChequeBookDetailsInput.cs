using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class GetAllChequeBookDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDetIDFilter { get; set; }
		public int? MinDetIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public string BANKIDFilter { get; set; }

		public string BankAccNoFilter { get; set; }

		public string FromChNoFilter { get; set; }

		public string ToChNoFilter { get; set; }

		public string BooKIDFilter { get; set; }

		public int? MaxVoucherNoFilter { get; set; }
		public int? MinVoucherNoFilter { get; set; }

		public DateTime? MaxVoucherDateFilter { get; set; }
		public DateTime? MinVoucherDateFilter { get; set; }

		public string NarrationFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class GetAllChequeBooksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string BANKIDFilter { get; set; }

		public string BankAccNoFilter { get; set; }

		public string FromChNoFilter { get; set; }

		public string ToChNoFilter { get; set; }

		public int? MaxNoofChFilter { get; set; }
		public int? MinNoofChFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
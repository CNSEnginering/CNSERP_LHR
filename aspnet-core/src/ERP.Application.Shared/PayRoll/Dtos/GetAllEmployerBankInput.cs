using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Dtos
{
    public class GetAllEmployerBankInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxEBankIDFilter { get; set; }
		public int? MinEBankIDFilter { get; set; }

		public string EBankNameFilter { get; set; }

		public int? ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
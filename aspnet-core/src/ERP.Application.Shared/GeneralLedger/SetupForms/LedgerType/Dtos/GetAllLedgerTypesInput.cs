using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.LedgerType.Dtos
{
    public class GetAllLedgerTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? LedgerIDFilter { get; set; }

		public string LedgerDescFilter { get; set; }

		public int? ActiveFilter { get; set; }



    }
}
using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class GetAllGlChequesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTenantIDFilter { get; set; }
		public int? MinTenantIDFilter { get; set; }

		public int? MaxDocIDFilter { get; set; }
		public int? MinDocIDFilter { get; set; }

		public int? TypeIDFilter { get; set; }

		public DateTime? MaxEntryDateFilter { get; set; }
		public DateTime? MinEntryDateFilter { get; set; }

		public DateTime? MaxChequeDateFilter { get; set; }
		public DateTime? MinChequeDateFilter { get; set; }

		public string ChequeNoFilter { get; set; }

		public double? MaxChequeAmtFilter { get; set; }
		public double? MinChequeAmtFilter { get; set; }

		public string ChequeStatusFilter { get; set; }

		public string PartyBankFilter { get; set; }

		public string ChequeRefFilter { get; set; }

		public string RemarksFilter { get; set; }

		public int? MaxLocationIDFilter { get; set; }
		public int? MinLocationIDFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxPartyIDFilter { get; set; }
		public int? MinPartyIDFilter { get; set; }

		public string BankIDFilter { get; set; }

		public int PostedFilter { get; set; }

		public string AUDTUSERFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreatedDateFilter { get; set; }
		public DateTime? MinCreatedDateFilter { get; set; }



    }
}
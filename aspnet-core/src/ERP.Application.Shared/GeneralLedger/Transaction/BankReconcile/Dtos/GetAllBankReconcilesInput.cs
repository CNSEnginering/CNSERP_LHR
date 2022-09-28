using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class GetAllBankReconcilesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		
		public string DocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string BankIDFilter { get; set; }

		public string BankNameFilter { get; set; }

		

       

    }
}
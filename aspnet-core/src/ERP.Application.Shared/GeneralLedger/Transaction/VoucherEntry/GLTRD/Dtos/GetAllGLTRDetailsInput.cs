using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos
{
    public class GetAllGLTRDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDetIDFilter { get; set; }
		public int? MinDetIDFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxSubAccIDFilter { get; set; }
		public int? MinSubAccIDFilter { get; set; }

		public string NarrationFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public string ChequeNoFilter { get; set; }



    }
}
using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.SalesReference.Dtos
{
    public class GetAllSalesReferencesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxRefIDFilter { get; set; }
		public int? MinRefIDFilter { get; set; }

		public string RefNameFilter { get; set; }

		public int ACTIVEFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }

		public DateTime? MaxCreatedDATEFilter { get; set; }
		public DateTime? MinCreatedDATEFilter { get; set; }

		public string CreatedUSERFilter { get; set; }
        public string RefType { get; set; }


    }
}
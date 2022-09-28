using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.SaleAccounts.Dtos
{
    public class GetAllSaleAccountsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public string TypeIDFilter { get; set; }

		public string SalesACCFilter { get; set; }

		public string SalesRetACCFilter { get; set; }

		public string COGSACCFilter { get; set; }

		public string ChAccountIDFilter { get; set; }

		public string DiscAccFilter { get; set; }

		public string WriteOffAccFilter { get; set; }

		public short? MaxActiveFilter { get; set; }
		public short? MinActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}
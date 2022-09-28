using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.ICOPT5.Dtos
{
    public class GetAllICOPT5Input : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxOptIDFilter { get; set; }
		public int? MinOptIDFilter { get; set; }

		public string DescpFilter { get; set; }
    }
}
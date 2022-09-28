using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.AssetRegistration.Dtos
{
    public class GetAllAssetRegistrationDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxAssetIDFilter { get; set; }
		public int? MinAssetIDFilter { get; set; }

		public DateTime? MaxDepStartDateFilter { get; set; }
		public DateTime? MinDepStartDateFilter { get; set; }

		public short? MaxDepMethodFilter { get; set; }
		public short? MinDepMethodFilter { get; set; }

		public decimal? MaxAssetLifeFilter { get; set; }
		public decimal? MinAssetLifeFilter { get; set; }

		public decimal? MaxBookValueFilter { get; set; }
		public decimal? MinBookValueFilter { get; set; }

		public decimal? MaxLastDepAmountFilter { get; set; }
		public decimal? MinLastDepAmountFilter { get; set; }

		public DateTime? MaxLastDepDateFilter { get; set; }
		public DateTime? MinLastDepDateFilter { get; set; }

		public decimal? MaxAccumulatedDepAmtFilter { get; set; }
		public decimal? MinAccumulatedDepAmtFilter { get; set; }



    }
}
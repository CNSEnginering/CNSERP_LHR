using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.AssetRegistration.Dtos
{
    public class GetAllAssetRegistrationForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxAssetIDFilter { get; set; }
		public int? MinAssetIDFilter { get; set; }

		public string FmtAssetIDFilter { get; set; }

		public string AssetNameFilter { get; set; }

		public string ItemIDFilter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public DateTime? MaxRegDateFilter { get; set; }
		public DateTime? MinRegDateFilter { get; set; }

		public DateTime? MaxPurchaseDateFilter { get; set; }
		public DateTime? MinPurchaseDateFilter { get; set; }

		public DateTime? MaxExpiryDateFilter { get; set; }
		public DateTime? MinExpiryDateFilter { get; set; }

		public int WarrantyFilter { get; set; }

		public short? AssetTypeFilter { get; set; }
		

		public decimal? MaxDepRateFilter { get; set; }
		public decimal? MinDepRateFilter { get; set; }

		public short? DepMethodFilter { get; set; }
		

		public string SerialNumberFilter { get; set; }

		public decimal? MaxPurchasePriceFilter { get; set; }
		public decimal? MinPurchasePriceFilter { get; set; }

		public string NarrationFilter { get; set; }

		public string AccAssetFilter { get; set; }

		public string AccDeprFilter { get; set; }

		public string AccExpFilter { get; set; }

		public DateTime? MaxDepStartDateFilter { get; set; }
		public DateTime? MinDepStartDateFilter { get; set; }

		public decimal? MaxAssetLifeFilter { get; set; }
		public decimal? MinAssetLifeFilter { get; set; }

		public decimal? MaxBookValueFilter { get; set; }
		public decimal? MinBookValueFilter { get; set; }

		public decimal? MaxLastDepAmountFilter { get; set; }
		public decimal? MinLastDepAmountFilter { get; set; }

		public DateTime? MaxLastDepDateFilter { get; set; }
		public DateTime? MinLastDepDateFilter { get; set; }

		public int DisolvedFilter { get; set; }

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
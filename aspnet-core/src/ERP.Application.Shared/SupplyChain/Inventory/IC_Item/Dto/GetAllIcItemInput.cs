using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Item.Dto
{
    public class GetAllIcItemInput : PagedAndSortedResultRequestDto
    {

        public string Filter { get; set; }
        public string ItemIdFilter { get; set; }
        public string DescpFilter { get; set; }
        public string Seg1IdFilter { get; set; }
        public string Seg2IdFilter { get; set; }
        public string Seg3IdFilter { get; set; }
        public DateTime? CreationDateFilter { get; set; }
        public int? ItemCtgFilter { get; set; }
        public int? ItemTypeFilter { get; set; }
        public int? ItemStatusFilter { get; set; }
        public string StockUnitFilter { get; set; }
        public int? PackingFilter { get; set; }
        public double? WeightFilter { get; set; }
        public bool? TaxableFilter { get; set; }
        public bool? SaleableFilter { get; set; }
        public bool? ActiveFilter { get; set; }
        public string BarcodeFilter { get; set; }
    }
}

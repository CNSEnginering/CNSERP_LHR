using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SaleQutation.Dtos
{
    public class OEQDDto : EntityDto
    {
        public int DetID { get; set; }

        public int LocID { get; set; }

        public int DocNo { get; set; }

        public int TransType { get; set; }

        public string ItemID { get; set; }
        public string TransName { get; set; }

        public string Unit { get; set; }

        public decimal? Conver { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Rate { get; set; }

        public decimal? Amount { get; set; }

        public string TaxAuth { get; set; }

        public int? TaxClass { get; set; }
        public string TaxClassDesc { get; set; }

        public double? TaxRate { get; set; }

        public double? TaxAmt { get; set; }

        public double? NetAmount { get; set; }

        public string Remarks { get; set; }
        public bool? Approved { get; set; }
    }
}
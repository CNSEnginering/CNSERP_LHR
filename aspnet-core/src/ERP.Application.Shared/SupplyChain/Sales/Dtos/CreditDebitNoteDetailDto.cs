using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.SupplyChain.Sales.Dtos
{
    public class CreditDebitNoteDetailDto : EntityDto<int?>
    {
        public int TenantId { get; set; }
        public int DetID { get; set; }

        public int LocID { get; set; }

        public int DocNo { get; set; }

        public string ItemID { get; set; }

        public string Unit { get; set; }

        public double? Conver { get; set; }

        public double? Qty { get; set; }

        public double? Rate { get; set; }

        public double? Amount { get; set; }

        public double? Disc { get; set; }

        public string TaxAuth { get; set; }

        public int? TaxClass { get; set; }

        public double? TaxRate { get; set; }

        public double? TaxAmt { get; set; }

        public string Remarks { get; set; }

        public double? NetAmount { get; set; }

    }
}

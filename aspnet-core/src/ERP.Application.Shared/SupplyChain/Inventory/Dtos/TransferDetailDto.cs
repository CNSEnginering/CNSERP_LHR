using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class TransferDetailDto : EntityDto<int?>
    {
        public int TenantId { get; set; }
        public int DetId { get; set; }
        public int FromLocId { get; set; }
        public int DocNo { get; set; }
        public string ItemID { get; set; }
        public string Unit { get; set; }
        public decimal? Conver { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public string Bundle { get; set; }
        public string LotNo { get; set; }
        public decimal MaxQty { get; set; }
    }
}

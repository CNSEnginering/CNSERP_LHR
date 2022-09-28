
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class AssemblyDetailDto : EntityDto
    {
        public int TenantId { get; set; }
        public int DetId { get; set; }
        public int LocId { get; set; }
        public int DocNo { get; set; }
        public string ItemId { get; set; }
        public string Unit { get; set; }
        public decimal Conver { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int TransType { get; set; }

    }
}
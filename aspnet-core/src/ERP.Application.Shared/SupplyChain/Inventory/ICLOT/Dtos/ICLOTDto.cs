using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.ICLOT.Dtos
{
    public class ICLOTDto : EntityDto
    {
        public int? TenantID { get; set; }

        public string LotNo { get; set; }

        public DateTime? ManfDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
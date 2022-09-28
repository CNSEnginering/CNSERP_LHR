using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.ICLOT.Dtos
{
    public class GetAllICLOTInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxTenantIDFilter { get; set; }
        public int? MinTenantIDFilter { get; set; }

        public string LotNoFilter { get; set; }

        public DateTime? MaxManfDateFilter { get; set; }
        public DateTime? MinManfDateFilter { get; set; }

        public DateTime? MaxExpiryDateFilter { get; set; }
        public DateTime? MinExpiryDateFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
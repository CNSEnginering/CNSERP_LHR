
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class ReorderLevelDto : EntityDto<int?>
    {
        public int LocId { get; set; }

        public string ItemId { get; set; }
        public string ItemName { get; set; }

        public decimal? MinLevel { get; set; }

		public decimal? MaxLevel { get; set; }

		public decimal? OrdLevel { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }
    }
}

using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class TransactionTypeDto : EntityDto<int?>
    {
		public string Description { get; set; }

		public bool Active { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string TypeId { get; set; }



    }
}

using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class ICELocationDto : EntityDto
    {
        public int? ParentId { get; set; }
        public string ParentDesc { get; set; }

        public int? TenantID { get; set; }

		public string LocationTitle { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string ApprovedBy { get; set; }

		public DateTime? ApprovedDate { get; set; }



    }
}
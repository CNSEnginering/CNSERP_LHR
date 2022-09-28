
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CostCenterDto : EntityDto
    {
		public string CCID { get; set; }

		public string CCName { get; set; }

		public string AccountID { get; set; }

        public string AccountName { get; set; }

        public int? SubAccID { get; set; }
        public string SubAccName { get; set; }

        public short? Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}
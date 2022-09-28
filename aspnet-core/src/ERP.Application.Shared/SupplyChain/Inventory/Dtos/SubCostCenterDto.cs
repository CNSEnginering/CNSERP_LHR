
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class SubCostCenterDto : EntityDto
    {
        public string CCID { get; set; }
        public string CCName { get; set; }
        public int SUBCCID { get; set; }

        public string SubCCName { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }
        public bool? Active { get; set; }

        public DateTime? CreateDate { get; set; }

        public string AccountId { get; set; }
        public string AccountName { get; set; }
    }
}
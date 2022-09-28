
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class InventoryGlLinkDto : EntityDto
    {
        public int LocID { get; set; }

        public string SegID { get; set; }
        public string LocName { get; set; }
        public string SegName { get; set; }
        public string AccRec { get; set; }
        public string AccRecDesc { get; set; }

        public string AccRet { get; set; }
        public string AccRetDesc { get; set; }

        public string AccAdj { get; set; }
        public string AccAdjDesc { get; set; }

        public string AccCGS { get; set; }
        public string AccCGSDesc { get; set; }

        public string AccWIP { get; set; }
        public string AccWIPDesc { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }



    }
}
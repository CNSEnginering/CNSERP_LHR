
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class ICLocationDto : EntityDto<int?>
    {

        public int LocID { get; set; }
        public int ELoc1 { get; set; }
        public int ELoc2 { get; set; }
        public int ELoc3 { get; set; }
        public int ELoc4 { get; set; }
        public int ELoc5 { get; set; }

        public string LocName { get; set; }

        public string LocShort { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public bool AllowRec { get; set; }

        public bool AllowNeg { get; set; }

        public bool IsParent { get; set; }

        public int? ParentID { get; set; }

        public bool Active { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }



    }
}

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditAssemblyDto : EntityDto<int?>
    {

        [Required]
        public int LocID { get; set; }

        public string LocDesc { get; set; }
        [Required]
        public int DocNo { get; set; }


        public DateTime? DocDate { get; set; }


        public string Narration { get; set; }


        public bool Posted { get; set; }


        public int? LinkDetID { get; set; }


        public string OrdNo { get; set; }


        public short? Active { get; set; }


        public string AudtUser { get; set; }


        public DateTime? AudtDate { get; set; }


        public string CreatedBy { get; set; }


        public DateTime? CreateDate { get; set; }

        public decimal OverHead { get; set; }

        public List<AssemblyDetailDto> AssemblyDetailDto { get; set; }

    }
}
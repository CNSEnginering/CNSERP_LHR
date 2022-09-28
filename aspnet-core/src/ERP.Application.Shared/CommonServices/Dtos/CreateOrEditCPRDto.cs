using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.CommonServices.Dtos
{
    public class CreateOrEditCPRDto : EntityDto<int?>
    {
        [Required]
        public int CprId { get; set; }

        [Required]
        public string CprNo { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}

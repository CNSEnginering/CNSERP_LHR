using System;
using Abp.Application.Services.Dto;

namespace ERP.Payroll.SlabSetup.Dtos
{
    public class SlabSetupDto : EntityDto
    {
        public int? TypeID { get; set; }

        public double? SlabFrom { get; set; }

        public double? SlabTo { get; set; }

        public double? Rate { get; set; }

        public double? Amount { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifyDate { get; set; }

    }
}
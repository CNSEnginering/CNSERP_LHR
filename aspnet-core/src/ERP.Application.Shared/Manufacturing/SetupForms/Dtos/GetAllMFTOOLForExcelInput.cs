using Abp.Application.Services.Dto;
using System;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetAllMFTOOLForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxTenantIDFilter { get; set; }
        public int? MinTenantIDFilter { get; set; }

        public string TOOLIDFilter { get; set; }

        public string TOOLDESCFilter { get; set; }

        public int? STATUSFilter { get; set; }

        public string TOOLTYIDFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
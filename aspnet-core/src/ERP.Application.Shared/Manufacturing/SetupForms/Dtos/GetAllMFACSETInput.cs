using Abp.Application.Services.Dto;
using System;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetAllMFACSETInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxTenantIDFilter { get; set; }
        public int? MinTenantIDFilter { get; set; }

        public string ACCTSETFilter { get; set; }

        public string DESCFilter { get; set; }

        public string WIPACCTFilter { get; set; }

        public string SETLABACCTFilter { get; set; }

        public string RUNLABACCTFilter { get; set; }

        public string OVHACCTFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
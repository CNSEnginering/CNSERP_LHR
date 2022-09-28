using Abp.Application.Services.Dto;
using System;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetAllMFTOOLTYInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TOOLTYIDFilter { get; set; }

        public string TOOLTYDESCFilter { get; set; }

        public bool? MaxSTATUSFilter { get; set; }
        public bool? MinSTATUSFilter { get; set; }

        public double? MaxUNITCOSTFilter { get; set; }
        public double? MinUNITCOSTFilter { get; set; }

        public string UNITFilter { get; set; }

        public string COMMENTSFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
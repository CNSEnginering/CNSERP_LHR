using Abp.Application.Services.Dto;
using System;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetAllMFRESMASInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string RESIDFilter { get; set; }

        public string RESDESCFilter { get; set; }

        public int? ACTIVEFilter { get; set; }

        public short? MaxCOSTTYPEFilter { get; set; }
        public short? MinCOSTTYPEFilter { get; set; }

        public decimal? MaxUNITCOSTFilter { get; set; }
        public decimal? MinUNITCOSTFilter { get; set; }

        public short? MaxUOMTYPEFilter { get; set; }
        public short? MinUOMTYPEFilter { get; set; }

        public string UNITFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
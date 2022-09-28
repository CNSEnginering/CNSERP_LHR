using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.UserLoc.CSUserLocH.Dtos
{
    public class GetAllCSUserLocHInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public short? MaxTypeIDFilter { get; set; }
        public short? MinTypeIDFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

    }
}
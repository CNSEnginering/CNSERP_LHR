using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Cader.Dtos
{
    public class GetAllCaderInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CADER_NAMEFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
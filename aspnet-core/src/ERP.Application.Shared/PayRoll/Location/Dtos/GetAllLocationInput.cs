using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Location.Dtos
{
    public class GetAllLocationInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxLocIDFilter { get; set; }

        public int? MinLocIDFilter { get; set; }

        public string LocationFilter { get; set; }

        public int ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }

        public DateTime? MinCreateDateFilter { get; set; }
    }
}

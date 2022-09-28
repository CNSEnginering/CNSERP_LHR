using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Designation.Dtos
{
    public class GetAllDesignationInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDesignationIDFilter { get; set; }

        public int? MinDesignationIDFilter { get; set; }

        public string DesignationFilter { get; set; }

        public int ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }

        public DateTime? MinCreateDateFilter { get; set; }
    }
}

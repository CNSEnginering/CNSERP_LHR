using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Grades.Dtos
{
    public class GetAllGradeInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxGradeIDFilter { get; set; }

        public int? MinGradeIDFilter { get; set; }

        public string GradeNameFilter { get; set; }

        public int ActiveFilter { get; set; }

        public int? MaxTypeFilter { get; set; }

        public int? MinTypeFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }

        public DateTime? MinCreateDateFilter { get; set; }
    }
}

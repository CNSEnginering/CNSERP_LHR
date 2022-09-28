using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Religion.Dtos
{
    public class GetAllReligionForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxReligionIDFilter { get; set; }

        public int? MinReligionIDFilter { get; set; }

        public string ReligionFilter { get; set; }

        public int ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }

        public DateTime? MinCreateDateFilter { get; set; }
    }
}

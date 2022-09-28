using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.CommonServices.Dtos
{
    public class GetAllCPRForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxCprIdFilter { get; set; }

        public int? MinCprIdFilter { get; set; }

        public string CprNoFilter { get; set; }

        public int ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }

        public DateTime? MinCreateDateFilter { get; set; }
    }
}

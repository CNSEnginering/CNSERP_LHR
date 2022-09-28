using Abp.Application.Services.Dto;
using System;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetAllMFWCMForExcelInput
    {
        public string Filter { get; set; }

        public string WCIDFilter { get; set; }

        public string WCESCFilter { get; set; }

        public double? MaxTOTRSCCOSTFilter { get; set; }
        public double? MinTOTRSCCOSTFilter { get; set; }

        public double? MaxTOTTLCOSTFilter { get; set; }
        public double? MinTOTTLCOSTFilter { get; set; }

        public string COMMENTSFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
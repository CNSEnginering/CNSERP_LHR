using Abp.Application.Services.Dto;
using System;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetAllMFAREAForExcelInput
    {
        public string Filter { get; set; }

        public string AREAIDFilter { get; set; }

        public string AREADESCFilter { get; set; }

        public short? MaxAREATYFilter { get; set; }
        public short? MinAREATYFilter { get; set; }

        public short? MaxSTATUSFilter { get; set; }
        public short? MinSTATUSFilter { get; set; }

        public string ADDRESSFilter { get; set; }

        public string CONTNAMEFilter { get; set; }

        public string CONTPOSFilter { get; set; }

        public string CONTCELLFilter { get; set; }

        public string CONTEMAILFilter { get; set; }

        public int? MaxLOCIDFilter { get; set; }
        public int? MinLOCIDFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
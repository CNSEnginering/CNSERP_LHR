using Abp.Application.Services.Dto;
using System;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetAllMFWCTOLForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxDetIDFilter { get; set; }
        public int? MinDetIDFilter { get; set; }

        public string WCIDFilter { get; set; }

        public string TOOLTYIDFilter { get; set; }

        public string TOOLTYDESCFilter { get; set; }

        public string UOMFilter { get; set; }

        public double? MaxREQQTYFilter { get; set; }
        public double? MinREQQTYFilter { get; set; }

        public double? MaxUNITCOSTFilter { get; set; }
        public double? MinUNITCOSTFilter { get; set; }

        public double? MaxTOTALCOSTFilter { get; set; }
        public double? MinTOTALCOSTFilter { get; set; }

    }
}
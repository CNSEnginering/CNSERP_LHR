using System;
using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class MFWCTOLDto : EntityDto
    {
        public int? DetID { get; set; }

        public string WCID { get; set; }

        public string TOOLTYID { get; set; }

        public string TOOLTYDESC { get; set; }

        public string UOM { get; set; }

        public double? REQQTY { get; set; }

        public double? UNITCOST { get; set; }

        public double? TOTALCOST { get; set; }

    }
}
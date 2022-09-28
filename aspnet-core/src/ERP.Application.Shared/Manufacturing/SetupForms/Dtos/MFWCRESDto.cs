using System;
using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class MFWCRESDto : EntityDto
    {
        public int? DetID { get; set; }

        public string WCID { get; set; }

        public string RESID { get; set; }

        public string RESDESC { get; set; }

        public string UOM { get; set; }

        public double? REQQTY { get; set; }

        public double? UNITCOST { get; set; }

        public double? TOTALCOST { get; set; }

    }
}
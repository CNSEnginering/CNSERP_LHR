using System;
using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class MFTOOLTYDto : EntityDto
    {
        public string TOOLTYID { get; set; }

        public string TOOLTYDESC { get; set; }

        public bool STATUS { get; set; }

        public double UNITCOST { get; set; }

        public string UNIT { get; set; }

        public string COMMENTS { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
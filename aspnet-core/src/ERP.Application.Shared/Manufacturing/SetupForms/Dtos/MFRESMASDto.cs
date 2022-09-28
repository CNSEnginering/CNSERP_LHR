using System;
using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class MFRESMASDto : EntityDto
    {
        public string RESID { get; set; }

        public string RESDESC { get; set; }

        public bool ACTIVE { get; set; }

        public short COSTTYPE { get; set; }

        public decimal UNITCOST { get; set; }

        public short UOMTYPE { get; set; }
        public short COSTBASIS { get; set; }

        public string UNIT { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
using System;
using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class MFAREADto : EntityDto
    {
        public string AREAID { get; set; }

        public string AREADESC { get; set; }

        public short AREATY { get; set; }

        public short STATUS { get; set; }

        public string ADDRESS { get; set; }

        public string CONTNAME { get; set; }

        public string CONTPOS { get; set; }

        public string CONTCELL { get; set; }

        public string CONTEMAIL { get; set; }

        public int LOCID { get; set; }
        public string LocDesc { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
        public bool? Active { get; set; }
    }
}
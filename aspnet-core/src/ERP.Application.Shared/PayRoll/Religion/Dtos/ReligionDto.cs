using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Religion.Dtos
{
    public class ReligionDto: EntityDto
    {
        public int ReligionID { get; set; }

        public string Religion { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public  DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}

using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Designation.Dtos
{
    public class DesignationDto: EntityDto
    {
        public int DesignationID { get; set; }

        public string Designation { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public  DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}

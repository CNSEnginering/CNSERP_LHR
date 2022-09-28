using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Location.Dtos
{
    public class LocationDto: EntityDto
    {
        public int LocID { get; set; }

        public string Location { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public  DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
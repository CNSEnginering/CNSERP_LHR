using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Grades.Dtos
{
    public class GradeDto: EntityDto
    {
        public int GradeID { get; set; }

        public string GradeName { get; set; }

        public int Type { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public  DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}

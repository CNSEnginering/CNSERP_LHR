using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.UserLoc.CSUserLocH.Dtos
{
    public class CSUserLocHDto : EntityDto
    {
        public short? TypeID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string AudtUser { get; set; }
        public string UserId { get; set; }

        public DateTime? AudtDate { get; set; }

    }
}
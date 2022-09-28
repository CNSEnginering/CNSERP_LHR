using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Cader.Dtos
{
    public class CaderDto : EntityDto<long>
    {
        public string CADER_NAME { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
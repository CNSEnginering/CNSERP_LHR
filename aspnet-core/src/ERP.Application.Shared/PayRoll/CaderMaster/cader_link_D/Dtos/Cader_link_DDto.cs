using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.CaderMaster.cader_link_D.Dtos
{
    public class Cader_link_DDto : EntityDto
    {
        public int CaderID { get; set; }

        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public string Type { get; set; }

        public string PayType { get; set; }

        public string Narration { get; set; }

    }
}
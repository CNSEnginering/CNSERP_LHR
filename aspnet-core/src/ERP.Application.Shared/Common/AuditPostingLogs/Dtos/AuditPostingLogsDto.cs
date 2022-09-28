
using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.AuditPostingLogs.Dtos
{
    public class AuditPostingLogsDto : EntityDto
    {
		public string Status { get; set; }

		public string IpAddress { get; set; }



    }
}
using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.AuditPostingLogs.Dtos
{
    public class GetAllAuditPostingLogsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string StatusFilter { get; set; }

		public string IpAddressFilter { get; set; }



    }
}
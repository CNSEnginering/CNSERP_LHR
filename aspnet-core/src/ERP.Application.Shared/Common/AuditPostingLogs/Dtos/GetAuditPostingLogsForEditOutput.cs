using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.AuditPostingLogs.Dtos
{
    public class GetAuditPostingLogsForEditOutput
    {
		public CreateOrEditAuditPostingLogsDto AuditPostingLogs { get; set; }


    }
}
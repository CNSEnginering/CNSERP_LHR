
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.AuditPostingLogs.Dtos
{
    public class CreateOrEditAuditPostingLogsDto : EntityDto<int?>
    {

        public int? TenantId { get; set; }
        public int DetId { get; set; }
        public string BookId { get; set; }
        [Required]
        [StringLength(AuditPostingLogsConsts.MaxStatusLength, MinimumLength = AuditPostingLogsConsts.MinStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(AuditPostingLogsConsts.MaxIpAddressLength, MinimumLength = AuditPostingLogsConsts.MinIpAddressLength)]
        public virtual string IpAddress { get; set; }
        public DateTime AuditDateTime { get; set; }
        public string SystemUser { get; set; }

    }
}
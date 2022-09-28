using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common.AuditPostingLogs
{
	[Table("AuditPostingLogs")]
    public class AuditPostingLogs : Entity , IMustHaveTenant
    {
		public int TenantId { get; set; }
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
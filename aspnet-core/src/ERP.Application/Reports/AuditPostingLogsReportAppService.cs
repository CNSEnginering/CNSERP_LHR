using Abp.Domain.Repositories;
using ERP.Common.AuditPostingLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports
{
    public class AuditPostingLogsReportAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<AuditPostingLogs> _auditPostingLogsRepository;
        public AuditPostingLogsReportAppService(IRepository<AuditPostingLogs> auditPostingLogsRepository)
        {
            _auditPostingLogsRepository = _auditPostingLogsRepository;
        }
        public List<AuditPostingReport> Getreport(string user, DateTime fromDate, DateTime toDate, string voucher)
        {
            var logs = from a in _auditPostingLogsRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookId == voucher && o.SystemUser == user
                       && o.AuditDateTime.Date >= fromDate.Date && o.AuditDateTime.Date <= toDate.Date
                       )
                       select new AuditPostingReport()
                       {
                           BookId = a.BookId,
                           DateTime = a.SystemUser,
                           IPAddress = a.IpAddress,
                           Status = a.Status,
                           SystemUser = a.SystemUser
                       };
            return logs.ToList();
        }
    }

    public class AuditPostingReport
    {
        public string BookId { get; set; }
        public string Status { get; set; }
        public string IPAddress { get; set; }
        public string DateTime { get; set; }
        public string SystemUser { get; set; }
    }
}

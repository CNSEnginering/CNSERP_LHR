using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Newtonsoft.Json;
using ERP.Common.AlertLog;
using ERP.Common.CSAlertInfo;
using ERP.Common.AlertLog.Dtos;

namespace ERP.Reports.Finance
{
    public class AlertLogReportAppService : ERPReportAppServiceBase , IAlertLogReportAppService
    {
        private readonly IRepository<CSAlertLog> _CSAlertLogRepository;
        private readonly IRepository<CSAlert> _CSAlertRepository;
       
        public AlertLogReportAppService(IRepository<CSAlertLog> CSAlertLogRepository,
           IRepository<CSAlert> CSAlertRepository
            )
        {
            _CSAlertLogRepository = CSAlertLogRepository;
            _CSAlertRepository = CSAlertRepository;
        }



        public List<CSAlertLogDto> GetAll()
        {
            var quer = from o in _CSAlertLogRepository.GetAll()
                       join p in _CSAlertRepository.GetAll() on new { o.AlertId, o.TenantId } equals new { p.AlertId, p.TenantId }

                       select new CSAlertLogDto
                       {
                           AlertId = o.AlertId,
                           SentDate = o.SentDate,
                           UserHost = o.UserHost,
                           LogInUser = o.LogInUser,
                           Active = o.Active,
                           AudtUser = o.AudtUser,
                           AudtDate = o.AudtDate,
                           CreatedBy = o.CreatedBy,
                           CreateDate = o.CreateDate,
                           AlertMessage = p.AlertBody

                       };

            string repJsonP = JsonConvert.SerializeObject(quer.ToList());

            return new List<CSAlertLogDto>
            (quer.ToList());
                
            



        }    
    }
}

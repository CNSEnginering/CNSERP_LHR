using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.AlertLog.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports
{
    public interface IAlertLogReportAppService : IApplicationService
    {
        List<CSAlertLogDto> GetAll();
    }
}

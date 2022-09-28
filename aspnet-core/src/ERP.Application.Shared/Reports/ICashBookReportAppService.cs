using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports
{
    public interface ICashBookReportAppService: IApplicationService
    {
        //List<CashBookDto> GetAll(DateTime? FromDate, DateTime? ToDate, string FromAccount, string ToAccount, int TenantID, bool CashBook);
    }
}

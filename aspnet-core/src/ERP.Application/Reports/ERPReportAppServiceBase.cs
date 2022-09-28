using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports
{
    [Produces("application/xml")]
    [WrapResult(WrapOnSuccess = false, WrapOnError = false)]
    public class ERPReportAppServiceBase : ERPAppServiceBase
    {

    }
}

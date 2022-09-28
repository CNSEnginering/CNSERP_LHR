using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Web.Host.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("DXXRDV")]
    public class DXWebDocumentViewerController : WebDocumentViewerController
    {
        public DXWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService) { }
        public override Task<IActionResult> Invoke()
        {
            //Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            return base.Invoke();
        }
    }
}
using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Web.Controllers
{
    public class HomeController : ERPControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}

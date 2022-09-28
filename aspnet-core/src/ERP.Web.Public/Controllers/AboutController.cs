using Microsoft.AspNetCore.Mvc;
using ERP.Web.Controllers;

namespace ERP.Web.Public.Controllers
{
    public class AboutController : ERPControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
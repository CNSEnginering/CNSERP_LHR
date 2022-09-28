using Microsoft.AspNetCore.Mvc;
using ERP.Web.Controllers;

namespace ERP.Web.Public.Controllers
{
    public class HomeController : ERPControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
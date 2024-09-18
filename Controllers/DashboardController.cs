using Microsoft.AspNetCore.Mvc;

namespace Nazox.Controllers.Dashboard
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}
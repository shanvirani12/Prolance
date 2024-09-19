using Microsoft.AspNetCore.Mvc;

namespace Prolance.Controllers
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
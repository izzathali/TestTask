using Microsoft.AspNetCore.Mvc;

namespace TestTask.WebApplication.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

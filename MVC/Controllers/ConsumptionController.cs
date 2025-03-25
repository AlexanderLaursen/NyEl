using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class ConsumptionController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}

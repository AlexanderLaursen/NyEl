using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        protected virtual string GetBearerToken()
        {
            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");

            if (bearerToken == null)
            {
                throw new UnauthorizedAccessException();
            }

            return bearerToken;
        }
    }
}

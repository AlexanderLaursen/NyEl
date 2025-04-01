using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        protected virtual BearerToken GetBearerToken()
        {
            BearerToken? bearerToken = HttpContext.Session.GetJson<BearerToken>("BearerToken");

            if (bearerToken == null)
            {
                throw new UnauthorizedAccessException();
            }

            return bearerToken;
        }
    }
}

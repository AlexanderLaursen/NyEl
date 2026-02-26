using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        // Retrives the BearerToken from the session
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

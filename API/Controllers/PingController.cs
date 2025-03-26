using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/ping")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("/secure")]
        public IActionResult SecurePing()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(userId);
        }
    }
}

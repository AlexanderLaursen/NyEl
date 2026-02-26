using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/v1/ping")]
    public class PingController : Controller
    {
        private readonly ILogger<PingController> _logger;
        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }

        // Ping endpoint open to all
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Ping...");
            return Ok();
        }

        // Ping endpoint that requires authentication
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecurePing()
        {
            _logger.LogInformation("Secure ping...");
            return Ok();
        }

        // Ping endpoint that requires authentication and a admin role
        [Authorize(Roles="Admins")]
        [HttpGet("admin")]
        public IActionResult AdminPing()
        {
            _logger.LogInformation("Admin ping...");
            return Ok();
        }
    }
}

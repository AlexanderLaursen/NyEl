﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/ping")]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> _logger;
        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Ping...");
            return Ok();
        }

        [Authorize]
        [HttpGet("/secure")]
        public IActionResult SecurePing()
        {
            _logger.LogInformation("Secure ping...");
            return Ok();
        }
    }
}

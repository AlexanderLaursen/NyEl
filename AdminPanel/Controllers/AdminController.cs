using System.Diagnostics;
using AdminPanel.Models;
using Common.Dtos.Login;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Services;
using MVC.Services.Interfaces;

namespace AdminPanel.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;
        private readonly CommonApiService _apiService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IAuthService authService, CommonApiService commonApiService)
        {
            _logger = logger;
            _authService = authService;
            _apiService = commonApiService;
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> AdminLogin(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View("AdminLogin");
            }

            Result<BearerToken> result = await _authService.LoginAsync(loginDto);

            if (!result.IsSuccess || result.Value?.AccessToken == null)
            {
                return View("AdminLogin");
            }

            Result<string> adminPing = await _apiService.GetAsync<string>("/ping/admin", new BearerToken { AccessToken = result.Value.AccessToken});

            if (!adminPing.IsSuccess)
            {
                return View("AdminLogin");
            }

            HttpContext.Session.SetJson("BearerToken", result.Value);
            HttpContext.Session.SetJson("Username", loginDto.Email);
            return RedirectToAction("Index", "AdminInvoice");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

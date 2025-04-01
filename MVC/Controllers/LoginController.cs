using Common.Dtos.Login;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Helpers;
using MVC.Services.Interfaces;
using Common.Dtos.User;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            Result<BearerToken> result = await _authService.LoginAsync(loginDto);

            if (!result.IsSuccess || result.Value?.AccessToken == null)
            {
                return View("Index");
            }

            HttpContext.Session.SetJson("BearerToken", result.Value);
            HttpContext.Session.SetJson("Username", loginDto.Email);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("BearerToken");
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }
}

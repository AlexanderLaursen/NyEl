using Common.Dtos.Login;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Helpers;
using MVC.Services.Interfaces;
using Common.Dtos.Consumer;
using Common.Dtos.User;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public LoginController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Result<BearerToken> result = await _authService.LoginAsync(loginDto);

            if (!result.IsSuccess || result.Value?.AccessToken == null)
            {
                return View();
            }

            HttpContext.Session.SetJson("Bearer", result.Value.AccessToken);

            Result<UserDto> userResult = await _userService.GetUserByEmailAsync(loginDto.Email);

            if (userResult.IsSuccess)
            {
                HttpContext.Session.SetJson("Username", loginDto.Email);
                HttpContext.Session.SetJson("UserId", userResult.Value?.UserId ?? string.Empty);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Bearer");
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
    }
}

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
        private readonly IUserService _userService;

        public LoginController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }

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

            HttpContext.Session.SetJson("Bearer", result.Value.AccessToken);

            Result<UserIdDto> userResult = await _userService.GetUserByEmailAsync(loginDto.Email);

            if (userResult.IsSuccess)
            {
                HttpContext.Session.SetJson("Username", loginDto.Email);
                HttpContext.Session.SetJson("UserId", userResult.Value.UserId);
                return RedirectToAction("Index", "Home");
            }

            return View("Index", "Home");
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Bearer");
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
    }
}

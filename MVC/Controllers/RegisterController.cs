using Common.Dtos.Login;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IAuthService _authService;

        public RegisterController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("/register")]
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterDataViewModel registerData)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            if (registerData.Password != registerData.ConfirmPassword)
            {
                return View("Index");
            }

            RegisterDto registerDto = registerData.Adapt<RegisterDto>();

            Result<bool> result = await _authService.RegisterAsync(registerDto);

            if (!result.IsSuccess)
            {
                return View("Index");
            }

            RegisterViewModel viewModel = new()
            {
                Email = registerDto.Email
            };

            return View("FullRegisterPage", viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpGet("/register-full")]
        public IActionResult FullRegisterPage(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("/register-full")]
        public async Task<IActionResult> RegisterFull(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("FullRegisterPage");
            }

            RegisterFullDto registerDto = registerViewModel.Adapt<RegisterFullDto>();

            Result<bool> resultDto = await _authService.RegisterFullAsync(registerDto);

            if (!resultDto.IsSuccess)
            {
                return View("Index");
            }

            return RedirectToAction("Index", "Login");
        }
    }
}

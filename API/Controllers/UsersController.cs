﻿using API.Repositories.Interfaces;
using Common.Dtos.Login;
using Common.Dtos.User;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly ICommonRepository<Consumer> _commonRepository;


        public UsersController(UserManager<AppUser> userManager, ILogger<UsersController> logger, ICommonRepository<Consumer> commonRepository)
        {
            _commonRepository = commonRepository;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return NotFound();
                }

                UserDto userDto = new() { UserId = user.Id };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the user.");
                return StatusCode(500);
            }
        }

        [HttpPost("/api/v1/register-full")]
        public async Task<IActionResult> RegisterUserFull(RegisterFullDto registerFullDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(registerFullDto.Email);

                if (user == null)
                {
                    return BadRequest("User does not exists.");
                }

                Consumer consumer = new()
                {
                    FirstName = registerFullDto.FirstName,
                    LastName = registerFullDto.LastName,
                    PhoneNumber = registerFullDto.PhoneNumber,
                    Email = registerFullDto.Email,
                    CPR = registerFullDto.CPR,
                    UserId = user.Id,
                };

                int result = await _commonRepository.AddAsync(consumer);

                if (result == 0)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");
                return StatusCode(500);
            }
        }
    }
}

using System.Net.Http.Headers;
using System.Text.Json;
using Common.Dtos.Login;
using Common.Exceptions;
using Common.Models;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class AuthService : CommonApiService, IAuthService
    {
        public const string LOGIN_URL = "/login";

        public AuthService(HttpClient httpClient, ILogger<CommonApiService> logger, IConfiguration configuration) : base(httpClient, logger, configuration)
        {
        }

        public async Task<Result<BearerToken>> LoginAsync(LoginDto loginDto)
        {
            return await PostCustomUrlAsync<BearerToken>("https://localhost:7231/login", loginDto);
        }

        public async Task<Result<bool>> RegisterAsync(RegisterDto registerDto)
        {
            return await PostCustomUrlAsync<bool>("https://localhost:7231/register", registerDto);
        }

        public async Task<Result<bool>> RegisterFullAsync(RegisterFullDto registerFullDto)
        {
            return await PostAsync<bool>("/register-full", registerFullDto);
        }
    }
}

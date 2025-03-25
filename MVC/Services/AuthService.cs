using Common.Dtos.Login;
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
            return await PostAsync<BearerToken>(LOGIN_URL, loginDto);
        }
    }
}

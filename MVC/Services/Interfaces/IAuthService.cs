using Common.Dtos.Login;
using Common.Models;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<Result<BearerToken>> LoginAsync(LoginDto loginDto);
    }
}
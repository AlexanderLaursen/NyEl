using Common.Dtos.User;
using Common.Models;

namespace MVC.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Result<UserDto>> GetUserByEmailAsync(string email);
    }
}
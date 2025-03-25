using Common.Dtos.User;

namespace API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public UserDto GetUserByEmail(string email);
    }
}
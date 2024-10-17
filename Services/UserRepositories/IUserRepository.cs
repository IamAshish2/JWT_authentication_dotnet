using JWT_AUTHENTICATION.Models;

namespace JWT_AUTHENTICATION.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUserName(string userName);
        Task<User> CreateUser(User user);
        //Task<User> UserExists(string userName);
    }
}

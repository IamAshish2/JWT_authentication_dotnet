using JWT_AUTHENTICATION.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWT_AUTHENTICATION.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUserName(string userName);
        Task<User> CreateUser(User user);
        Task<User> GetById(Guid userId);
        //Task<bool> Logout(Guid userId);
    }
}

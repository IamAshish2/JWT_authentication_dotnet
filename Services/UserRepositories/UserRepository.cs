using JWT_AUTHENTICATION.Data;
using JWT_AUTHENTICATION.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JWT_AUTHENTICATION.Services.UserRepositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<User> CreateUser(User user)
        {
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            _context.SaveChanges(); 
            return Task.FromResult(user);
        }

        // check this method for ! non-nullable references later->>>>>
        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_context.Users.Where(u => u.Email == email).FirstOrDefault())!;
        }

        public Task<User> GetById(Guid userId)
        {
            return Task.FromResult(_context.Users.Where(u => u.Id == userId).FirstOrDefault())!;
        }

        public Task<User> GetByUserName(string userName)
        {
            return Task.FromResult(_context.Users.Where(u => u.UserName == userName).FirstOrDefault())!;
        }

        public Task Logout(Guid userId)
        {
            User? user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            _context.Remove(user);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}

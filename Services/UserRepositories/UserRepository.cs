using JWT_AUTHENTICATION.Data;
using JWT_AUTHENTICATION.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

        // complete logout operations later on
        //[HttpPost("logout")]
        //public  Task<bool> Logout(Guid userId)
        //{
        //    // If using refresh tokens, you could remove the token or mark it as invalid
        //    var userTokens = _context.RefreshTokens.Where(t => t.UserId == userId);
        //    if (userTokens.Any())
        //    {
        //        _context.RefreshTokens.RemoveRange(userTokens);
        //    }


        //    return Task.FromResult(Save());
        //}

        private bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

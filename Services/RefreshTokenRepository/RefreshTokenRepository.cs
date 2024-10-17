using JWT_AUTHENTICATION.Data;
using JWT_AUTHENTICATION.Models;

namespace JWT_AUTHENTICATION.Services.RefreshTokenRepository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task Create(RefreshToken refreshToken)
        {
            refreshToken.UserId = Guid.NewGuid();
            _context.Add(refreshToken);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetByToken(string Token)
        {
            RefreshToken refreshToken = _context.RefreshTokens.FirstOrDefault(r => r.Token == Token);
            return Task.FromResult(refreshToken);
        }
    }
}

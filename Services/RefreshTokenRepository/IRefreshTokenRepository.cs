using JWT_AUTHENTICATION.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace JWT_AUTHENTICATION.Services.RefreshTokenRepository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByToken(string Token);
        Task Create(RefreshToken refreshToken);  
    }
}

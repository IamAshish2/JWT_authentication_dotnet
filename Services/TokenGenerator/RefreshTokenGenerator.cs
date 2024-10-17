using JWT_AUTHENTICATION.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWT_AUTHENTICATION.Services.TokenGenerator
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration, JwtTokenGenerator jwtTokenGenerator)
        {
            _configuration = configuration;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public string GenerateRefreshToken()
        {
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpiryMinutes);

            return _jwtTokenGenerator.GenerateToken(
                 _configuration.RefreshTokenKey,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime
                );
        }
    }
}

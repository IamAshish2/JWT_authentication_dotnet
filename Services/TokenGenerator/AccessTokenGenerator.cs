using JWT_AUTHENTICATION.Models;
using System.Security.Claims;
using JWT_AUTHENTICATION.Services.TokenGenerator;

namespace JWT_AUTHENTICATION.Services.TokenGenerator
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public AccessTokenGenerator(AuthenticationConfiguration configuration, JwtTokenGenerator jwtTokenGenerator)
        {
            _configuration = configuration;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public string GenerateAccessToken(User user)
        {
            List<Claim> claims =
            [
                new Claim("id",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName)
            ];

            DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpiryMinutes);
            // Calculate token expiration time based on the current time
            //DateTime expirationTime = DateTime.UtcNow.AddMinutes(0.25);

            return _jwtTokenGenerator.GenerateToken(
                _configuration.AccessTokenKey,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime,
                claims
                );
        }
    }
}

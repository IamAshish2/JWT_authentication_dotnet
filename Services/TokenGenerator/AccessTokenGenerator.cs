using JWT_AUTHENTICATION.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_AUTHENTICATION.Services.TokenGenerator
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;

        public AccessTokenGenerator(AuthenticationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GeneratorToken(User user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.AccessTokenKey));
            SigningCredentials credentials = new(key,SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim("id",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName)
            ];


            // Calculate token expiration time based on the current time
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(30);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration.Issuer, // issuer
                audience: _configuration.Audience, // audience
                claims, // claims contain the login detail credentials
                notBefore: DateTime.UtcNow, // the time the jwt token was created
                expires: expirationTime, // validity of the jwt token
                signingCredentials: credentials // the signin credentials -> key
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

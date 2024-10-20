using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_AUTHENTICATION.Services.TokenGenerator
{
    public class JwtTokenGenerator
    {
        public string GenerateToken(string secretKey, string issuer, string audience, DateTime utcExpirationTime,
            IEnumerable<Claim>? claims = null)
        {

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer, // issuer
                audience: audience, // audience
                claims, // claims contain the login detail credentials
                notBefore: DateTime.UtcNow, // the time the jwt token was created
                expires: utcExpirationTime, // validity of the jwt token
                signingCredentials: credentials // the signin credentials -> key
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
